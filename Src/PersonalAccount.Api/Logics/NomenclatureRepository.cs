using System;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Logics;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

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
        // Список сотрудников в пачке
        var batchItems = transactions
                        .GroupBy(x => new { x.EmploeeCode, x.EmploeeName })
                        .Select(x => new { x.Key.EmploeeCode, x.Key.EmploeeName })
                        .ToList();

        if (!batchItems.Any()) return Enumerable.Empty<NomenclatureModel>();

        // Список существующих сотрудников
        var categoryCodes = batchItems.Select(x => x.EmploeeCode).ToList();
        var entities = _context.Emploees
                        .Where(x => categoryCodes.Contains(x.ExternalCode))
                        .Select(x => new NomenclatureModel()
                        {
                            Id = x.Id,
                            Name = x.Name ?? string.Empty,
                            ExternalCode = x.ExternalCode,

                        })
                        .ToList();

        // Получаем список новых сотрудников
        var newest = from p in batchItems
                     join s in entities on p.EmploeeCode equals s.ExternalCode into leftTable
                     from l in leftTable.DefaultIfEmpty()
                     where l == null
                     select new NomenclatureModel
                     {
                         ExternalCode = p.EmploeeCode,
                         Name = p.EmploeeName,
                     };

        return entities.Union(newest);
    }

    /// <inheritdoc/>
    public Task<IEnumerable<NomenclatureModel>> GetRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Save(IEnumerable<NomenclatureModel> items, LoadingSettingsModel options)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<bool> SaveAsync(IEnumerable<NomenclatureModel> items, LoadingSettingsModel options, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
