using PersonalAccount.Common.Core;
using PersonalAccount.Common.Logics;
using PersonalAccount.Data;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ICategoryRepository"/>
/// </summary>
public class CategoryRepository(PersonalAccountContext context) : Buffer<CategoryModel>, ICategoryRepository
{
    private readonly PersonalAccountContext _context = context;

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
        var entities = _context.Categories
                        .Where(x => batchItems.Any(y => y.CategoryCode == x.ExternalCode))
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
    public bool Save(IEnumerable<CategoryModel> categories)
    {
        if(!categories.Any()) return false;
        try
        {
            var items = categories.Select(x => new Category()
            {
                Name = x.Name,
                ExternalCode = x.ExternalCode,
                CompanyId = x.Owner.Id
            });

            _context.AddRange( items );
            _context.SaveChanges();

            return true;
        }
        catch(Exception ex)
        {
            Log.Error($"Невозможно сохранить данные по категориям!\n{ex.Message}{ex.InnerException?.Message}");
            return false;
        }
    }

    /// <InheritDoc/>
    public async Task<bool> SaveAsync(IEnumerable<CategoryModel> categories, CancellationToken token)
        => await Task.Run( () => Save( categories), token);
}
