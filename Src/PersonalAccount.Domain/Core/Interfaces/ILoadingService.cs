using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Domain.Core.Interfaces;

public interface ILoadingService
{   
    /// <summary>
    /// Записать данные.
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="entries"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool Push(
        Organisation organisation,
        IEnumerable<DtoJournalEntry> entries,
        CancellationToken token
    );

    /// <summary>
    /// Записать данныею
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="entries"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> PushAsync(
        Organisation organisation,
        IEnumerable<DtoJournalEntry> entries,
        CancellationToken token
    );
}
