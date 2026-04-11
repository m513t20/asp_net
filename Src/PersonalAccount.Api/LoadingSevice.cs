using System;
using Microsoft.Data.SqlClient;
using PersonalAccount.Data.Logics;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api;

/// <summary>
/// Сревис для загрузки и сохранения журныальных записей.
/// </summary>
public class LoadingSevice : ILoadingService
{
    private ILoadSettingsRepository _settingsReposity;

    private PersonalAccountContext _context;

    private SqlConnection _connection;

    public LoadingSevice(ILoadSettingsRepository settingsRepository, PersonalAccountContext context, SqlConnection connection) 
    {
        _settingsReposity = settingsRepository;
        _context = context;
        _connection = connection;
    }

    /// <summary>
    /// Вставить записи
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="entries"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool Push(Domain.Models.Organisation organisation, IEnumerable<DtoJournalEntry> entries, CancellationToken token)
    {
        
        var settings = _settingsReposity.LoadAsync(organisation, token).Result
                ?? new LoadSettings()
                {
                    UserOrganisation = organisation, PackSize = 1000, StartPosition = 1
                };
        var firstTransaction = entries.FirstOrDefault();
        if (firstTransaction is null)
            return false;

        // Отбрасываем
        var innerTransactions = entries.Where(x => x.Id >= settings.StartPosition);

        // Сохраняем
        var repo = new DtoJournalEntryRepository(_context, settings);
        var rows = repo.GetRows(_connection, settings).GetAwaiter().GetResult();
        var saving = repo.SaveJournal(rows);
        
        // Обновляем настройки
        var lastCode = innerTransactions.OrderBy(x => x.Id).First().Id;
        settings.StartPosition = lastCode; 
        var task = Task.Run( () => _settingsReposity.SaveAsync(settings, token), token);
        Task.WaitAll(task, saving);
        return true;
    }

    public async Task<bool> PushAsync(Domain.Models.Organisation organisation, IEnumerable<DtoJournalEntry> entries, CancellationToken token)
        => await Task.Run(() => Push(organisation, entries, token));
}
