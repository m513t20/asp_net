using System;
using System.Data.Common;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

public interface IReader<T, TId> : IHandler<T> where T: IDto
{
    /// <summary>
    /// Получение данных по конкретному ID.
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="options"></param>
    /// <param name="requiredId"></param>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetRowsById(DbConnection connection, LoadingSettingsModel options, TId requiredId);

}
