using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Domain.Core.Interfaces;

/// <summary>
/// Интерфейс для записи транзакций с клиента в плоскую таблицу.
/// </summary>
public interface ISavingService
{
    /// <summary>
    /// Сохранить данные.
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="transactions"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool Save(IEnumerable<Transaction> transactions);

    /// <summary>
    /// Сохранить данные.
    /// </summary>
    /// <param name="organisation"></param>
    /// <param name="transactions"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> SaveAsync(IEnumerable<Transaction> transactions, CancellationToken token
    );
}
