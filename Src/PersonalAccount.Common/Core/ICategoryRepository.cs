using System;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для работы с категориями. Выборка, запись, буфер.
/// </summary>
/// <typeparam name="TSource"> Исходный тип IDto </typeparam>
/// <typeparam name="TDest"> Конечный тип </typeparam>
public interface ICategoryRepository : IHandler<JournalRowDto>, IBuffer<CategoryModel>
{
    /// <summary>
    /// Получить список новых категорий
    /// </summary>
    /// <param name="transactions"> Транзакции </param>
    /// <param name="options"> Настройки </param>
    /// <returns></returns>
    public IEnumerable<CategoryModel>  GetFreshRows(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options);

    /// <summary>
    /// Ассинхронный вариант получения списка новых категорий.
    /// </summary>
    /// <param name="transactions"> Транзакции. </param>
    /// <param name="options"> Настройки. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task<IEnumerable<CategoryModel>> GetFreshRowsAsync(IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options, CancellationToken token);

    /// <summary>
    /// Сохранить записи в базе данных и заполнить буфер.
    /// </summary>
    /// <param name="categories"> Список новых категорий. </param>
    /// <returns></returns>
    public bool Save(IEnumerable<CategoryModel> categories);

    /// <summary>
    /// Ассинхронный вариант записи в базе данных.
    /// </summary>
    /// <param name="categories"> Список новых категорий. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task<bool> SaveAsync(IEnumerable<CategoryModel> categories, CancellationToken token);
}
