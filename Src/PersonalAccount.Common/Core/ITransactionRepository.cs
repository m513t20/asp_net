using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Репозиторий для работы с транзакциями
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Сформировать и записать набор транзакций с раскладкой
    /// </summary>
    /// <param name="transactions"> Исходный "плоский" список транзакци. </param>
    /// <param name="options"></param>
    public bool Push(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options);

    /// <summary>
    /// Ассинхронный вариант записи набора транзакций.
    /// </summary>
    /// <param name="transactions"> Исходный "плоский" список транзакци. </param>
    /// <param name="options"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> PushAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token);
}
