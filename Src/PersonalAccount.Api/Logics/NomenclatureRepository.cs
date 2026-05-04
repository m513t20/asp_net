using System;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Logics;
using PersonalAccount.Data;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Репозиторий для работы с номенклатурой.
/// </summary>
public class NomenclatureRepository(PersonalAccountContext context) : Buffer<NomenclatureModel>,
                IBufferedRepository<JournalRowDto, NomenclatureModel>, IDisposable
{
    private readonly PersonalAccountContext _context = context;

    /// <inheritdoc/>
    public void Dispose()
    {
        _buffer.Clear();
    }

    /// <inheritdoc/>
    public IEnumerable<NomenclatureModel> GetRows(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        // Список номенклатуры в пачке
        var batchItems = transactions
                        .GroupBy(x => new { x.ProductCode, x.ProductName, x.CategoryCode })
                        .Select(x => new { x.Key.ProductCode, x.Key.ProductName, x.Key.CategoryCode })
                        .ToList();

        if (!batchItems.Any()) return Enumerable.Empty<NomenclatureModel>();

        // Список существующей номенклатуры
        var externalCodes = batchItems.Select(x => x.ProductCode).ToList();
        var categoryCodes = batchItems.Select(x => x.CategoryCode).ToList();

        // Внимание! Тут можно оптимизировать запрос.
        
        var categories = _context.Categories
                        .Where(x => categoryCodes.Contains(x.ExternalCode))
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

        var entities = _context.Nomenclatures
                        .Where(x => externalCodes.Contains(x.ExternalCode))
                        .ToList()
                        .Select(x => new NomenclatureModel()
                        {
                            Id = x.Id,
                            Name = x.Name ?? string.Empty,
                            ExternalCode = x.ExternalCode,
                            // Категория
                            Category =
                                    categories.FirstOrDefault(y => y.Id == x.CategoryId) ?? new CategoryModel()
                        })
                        .ToList();

        // Проверка. Вся номенклатура должна быть связана с категориями
        if (entities.Any(x => x.Category.Id == Guid.Empty)) throw new InvalidOperationException("Некорретно выполнена последовательность. Нет связи с категорией при формировании списка номенклатуры!");

        // Получаем список новой номенклатуры
        var newest = from p in batchItems
                     join s in entities on p.ProductCode equals s.ExternalCode into leftTable
                     from l in leftTable.DefaultIfEmpty()
                     where l == null
                     select new NomenclatureModel
                     {
                         ExternalCode = p.ProductCode,
                         Name = p.ProductName,
                         // Категория
                         Category =
                                    categories.FirstOrDefault(y => y.ExternalCode == p.CategoryCode) ?? new CategoryModel()
                     };

        // Проверка. Вся номенклатура должна быть связана с категориями
        if (newest.Any(x => x.Category.Id == Guid.Empty))  throw new InvalidOperationException("Некорретно выполнена последовательность. Нет связи с категорией при формировании списка номенклатуры!");

        return entities.Union(newest);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NomenclatureModel>> GetRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
        => await Task.Run(() => GetRows(transactions, options), token);


    /// <inheritdoc/>
    public bool Save(IEnumerable<NomenclatureModel> items, LoadingSettingsModel options)
    {
        if (!items.Any()) return false;
        try
        {
            // Получаем новые записи
            var newest = items
                .Where(x => x.Id == Guid.Empty)
                .Select(x => new Nomenclature()
                {
                    Name = x.Name,
                    ExternalCode = x.ExternalCode,
                });

            // Записываем
            _context.AddRange(newest);
            _context.SaveChanges();

            // Добавляем в буфер
            var key = new BufferKey(options, typeof(NomenclatureModel));
            Save(key, items);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Невозможно сохранить данные по категориям!\n{ex.Message}{ex.InnerException?.Message}");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SaveAsync(IEnumerable<NomenclatureModel> items, LoadingSettingsModel options, CancellationToken token)
        => await Task.Run(() => Save(items, options), token);
}
