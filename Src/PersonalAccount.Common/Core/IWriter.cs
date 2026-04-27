using System;
using PersonalAccount.Domain.Core;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Универсальный интерфейс для записи в бд
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IWriter <TData, TId> where TData : IDto
{
    /// <summary>
    /// Записать данные
    /// </summary>
    /// <param name="companyId"> Уникальный код подразделения </param>
    /// <param name="data"> Список транзакций </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool Push( 
        TId Id,
        IEnumerable<TData> data);

    /// <summary>
    /// Записать данные  (асинхронно)
    /// </summary>
    /// <param name="branchId"> Уникальный код подразделения </param>
    /// <param name="data"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> PushAsync( 
        TId Id,
        IEnumerable<TData> data,
        CancellationToken token);      

}
