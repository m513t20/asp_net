using System;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Абстрактный интерфейс для репозиторпия с буферизацией.
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface IBufferedRepository<TSource, TDest> : IHandler<TSource>, IBuffer<TDest>
where   
    TSource : IDto
where    
    TDest : DomainModel
{
     /// <summary>
    /// Получить список новых моделей <see cref="TDest"/>
    /// </summary>
    /// <param name="transactions"> Транзакции </param>
    /// <param name="options"> Настройки </param>
    /// <returns></returns>
    public IEnumerable<TDest>  GetRows(IEnumerable<TSource> transactions, LoadingSettingsModel options);

    /// <summary>
    /// Ассинхронный вариант получения списка новых моделей <see cref="TDest"/>.
    /// </summary>
    /// <param name="transactions"> Транзакции. </param>
    /// <param name="options"> Настройки. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task<IEnumerable<TDest>> GetRowsAsync(IEnumerable<TSource> transactions, LoadingSettingsModel options, CancellationToken token);

    /// <summary>
    /// Сохранить записи в базе данных и заполнить буфер.
    /// </summary>
    /// <param name="items"> Список сущностей. </param>
    /// <returns></returns>
    public bool Save(IEnumerable<TDest> items, LoadingSettingsModel options);

    /// <summary>
    /// Ассинхронный вариант записи в базе данных.
    /// </summary>
    /// <param name="items"> Список сущностей. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task<bool> SaveAsync(IEnumerable<TDest> items, LoadingSettingsModel options, CancellationToken token);
}
