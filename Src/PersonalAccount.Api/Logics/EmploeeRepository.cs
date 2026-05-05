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
/// Репозиторий для работы с сотрудниками.
/// </summary>
/// <param name="context"></param>
public class EmploeeRepository(PersonalAccountContext context) : Buffer<EmploeeModel>,
                IBufferedRepository<JournalRowDto, EmploeeModel>, IDisposable
{
    private readonly PersonalAccountContext _context = context;


    /// <inheritdoc/>
    public void Dispose()
    {
        _buffer.Clear();
    }

    /// <inheritdoc/>
    public IEnumerable<EmploeeModel> GetRows(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        // Список сотрудников в пачке
        var batchItems = transactions
                        .Where(x => x.EmploeeCode > 0)
                        .GroupBy(x => new { x.EmploeeCode, x.EmploeeName })
                        .Select(x => new { x.Key.EmploeeCode, x.Key.EmploeeName })
                        .ToList();

        if (!batchItems.Any()) return Enumerable.Empty<EmploeeModel>();

        // Список существующих сотрудников
        var externalCodes = batchItems.Select(x => x.EmploeeCode).ToList();
        var entities = _context.Emploees
                        .Where(x => externalCodes.Contains(x.ExternalCode))
                        .Select(x => new EmploeeModel()
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

        // Получаем список новых сотрудников
        var newest = from p in batchItems
                     join s in entities on p.EmploeeCode equals s.ExternalCode into leftTable
                     from l in leftTable.DefaultIfEmpty()
                     where l == null
                     select new EmploeeModel
                     {
                         ExternalCode = p.EmploeeCode,
                         Name = p.EmploeeName,
                         Owner = new CompanyModel()
                         {
                             // Код организации
                             Id = options.Branch.Owner.Id
                         }
                     };

        return entities.Union(newest);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EmploeeModel>> GetRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token)
           => await Task.Run(() => GetRows(transactions, options), token);

    /// <inheritdoc/>
    public bool Save(IEnumerable<EmploeeModel> items, LoadingSettingsModel options)
    {
        if (!items.Any()) return false;
        try
        {
            // Получаем новые записи
            var newest = items
                .Where(x => x.Id == Guid.Empty)
                .Select(x => new Emploee()
                {
                    Name = x.Name,
                    ExternalCode = x.ExternalCode,
                    CompanyId = x.Owner.Id
                }).ToList();

            // Записываем
            _context.AddRange(newest);
            _context.SaveChanges();

            // Добавляем в буфер
            var key = new BufferKey(options, typeof(EmploeeModel));
            var updated = items.Join(
                newest,
                item => item.ExternalCode,
                newItem => newItem.ExternalCode,
                (item, newItem) => new { item, newItem }
            ).Select(x =>
            {
                x.item.Id = x.newItem.Id;
                return x.item;
            }).ToList();
            Save(key, updated);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Невозможно сохранить данные по категориям!\n{ex.Message}{ex.InnerException?.Message}");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SaveAsync(IEnumerable<EmploeeModel> items, LoadingSettingsModel options, CancellationToken token)
      => await Task.Run(() => Save(items, options), token);
}
