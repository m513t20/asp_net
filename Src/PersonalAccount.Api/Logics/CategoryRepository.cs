using PersonalAccount.Common.Core;
using PersonalAccount.Common.Logics;
using PersonalAccount.Data;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Репозиторий для работы с категориями.
/// </summary>
public class CategoryRepository(PersonalAccountContext context) : Buffer<CategoryModel>,
                IBufferedRepository<JournalRowDto, CategoryModel>, IDisposable
{
    private readonly PersonalAccountContext _context = context;

    /// <inheritdoc/>
    public void Dispose()
    {
        _buffer.Clear();
    }

    /// <InheritDoc/>
    public IEnumerable<CategoryModel> GetRows(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        // Список категории в пачке
        var batchItems = transactions
                        .GroupBy(x => new { x.CategoryCode, x.CategoryName })
                        .Select(x => new { x.Key.CategoryCode, x.Key.CategoryName })
                        .ToList();

        if (!batchItems.Any()) return Enumerable.Empty<CategoryModel>();

        // Список существующих категорий
        var externalCodes = batchItems.Select(x => x.CategoryCode).ToList();
        var entities = _context.Categories
                        .Where(x => externalCodes.Contains(x.ExternalCode))
                        .Select(x => new CategoryModel()
                        {
                            Id = x.Id,
                            Name = x.Name ?? string.Empty,
                            ExternalCode = x.ExternalCode,
                            Owner = new CompanyModel()
                            {
                                // Код организации
                                Id = options.Branch.Owner.Id
                            }
                        })
                        .ToList();

        // Получаем список новых категорий
        var newest = from p in batchItems
                   join s in entities on p.CategoryCode equals s.ExternalCode into leftTable
                   from l in leftTable.DefaultIfEmpty()
                   where l == null
                   select new CategoryModel
                   {
                       ExternalCode = p.CategoryCode,
                       Name = p.CategoryName,
                       Owner = new CompanyModel()
                       {
                           // Код организации
                           Id = options.Branch.Owner.Id
                       }
                   };

        return entities.Union(newest);
    }

    /// <InheritDoc/>
    public async Task<IEnumerable<CategoryModel>> GetRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
        => await Task.Run( () => GetRows( transactions, options), token);

    /// <InheritDoc/>
    public bool Save(IEnumerable<CategoryModel> items, LoadingSettingsModel options)
    {
        if(!items.Any()) return false;
        try
        {
            // Получаем новые записи
            var newest = items
                .Where(x => x.Id == Guid.Empty)
                .Select(x => new Category()
                {
                    Name = x.Name,
                    ExternalCode = x.ExternalCode,
                    CompanyId = x.Owner.Id
                });

            // Записываем
            _context.AddRange( newest );
            _context.SaveChanges();

            // Добавляем в буфер
            var key = new BufferKey( options, typeof(CategoryModel));
            Save( key, items );

            return true;
        }
        catch(Exception ex)
        {
            Log.Error($"Невозможно сохранить данные по категориям!\n{ex.Message}{ex.InnerException?.Message}");
            return false;
        }
    }

    /// <InheritDoc/>
    public async Task<bool> SaveAsync(IEnumerable<CategoryModel> items, LoadingSettingsModel options, CancellationToken token)
        => await Task.Run( () => Save( items, options ), token);
}
