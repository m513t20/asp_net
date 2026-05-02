using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Сервис загрузки данных из клиента.
/// </summary>
public interface ILoadingFromClientService
{
    /// <summary>
    /// Записать данные
    /// </summary>
    /// <param name="companyId"> Уникальный код подразделения </param>
    /// <param name="transactions"> Список транзакций </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public bool Push( 
        Guid branchId,
        IEnumerable<JournalRowDto> transactions);

    /// <summary>
    /// Записать данные  (асинхронно)
    /// </summary>
    /// <param name="branchId"> Уникальный код подразделения </param>
    /// <param name="transactions"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> PushAsync( 
        Guid branchId,
        IEnumerable<JournalRowDto> transactions,
        CancellationToken token);      

    /// <summary>
    /// Получить текущие настройки
    /// </summary>
    /// <param name="branchId"> Уникальный код подразделения </param>
    /// <returns></returns>
    public LoadingSettingsModel GetSettings(Guid branchId) ;  

    /// <summary>
    /// Получить текущие настройки (асинхронно)
    /// </summary>
    /// <param name="branchId"> Уникальный код подразделения. </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<LoadingSettingsModel> GetSettingsAsync(Guid branchId,
      CancellationToken token);        
}
