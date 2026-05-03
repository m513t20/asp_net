using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Отдельный тип для определения ключа
/// </summary>
/// <param name="settings"> Настройки. </param>
/// <param name="type"> Связанный тип данных </param>
public record BufferKey(LoadingSettingsModel settings, Type type);

public interface IBuffer<T> where T : DomainModel
{
    /// <summary>
    /// Сохранить данные в буфере.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="values"> Набор значений. </param>
    /// <returns></returns>
    public void Save(BufferKey key, IEnumerable<T> values);

    /// <summary>
    /// Ассинхронный вариант записи данных в буфере.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="values"> Набор значений. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task SaveAsync(BufferKey key, IEnumerable<T> values, CancellationToken token);

    /// <summary>
    /// Получить данные из буфера.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <returns></returns>
    public IEnumerable<T>? Get(BufferKey key);

    /// <summary>
    /// Ассинхронный вариант получения данных из буфера.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public Task<IEnumerable<T>?> GetAsync(BufferKey key, CancellationToken token);
}
