using System;
using System.Data.Common;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для работы с серверными данными
/// </summary>
public interface IServerRepository<T>: IHandler<T> where T: IDto
{
    /// <summary>
    /// Сохранить данные в blob таблице
    /// </summary>
    /// <param name="connection"> Текущее соединение. </param>
    /// <param name="transactions"> Набор полученных транзакций. </param>
    /// <param name="options"> Набор настроек. </param>
    /// <returns> Обновленный вариант настроек. </returns>
    public Task<LoadingSettingsModel?> SaveRowsAsync(DbConnection connection, IEnumerable<T> transactions,  LoadingSettingsModel options);
}
