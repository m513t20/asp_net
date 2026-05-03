using System;
using System.Collections.Concurrent;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Logics;

/// <summary>
/// Реализация интерфейса <see cref="IBuffer"/>
/// </summary>
/// <typeparam name="T"> Связанный доменный тип данных. </typeparam>
public class Buffer<T> : IBuffer<T> where T : DomainModel
{
    protected readonly ConcurrentDictionary<BufferKey, IEnumerable<T>> _buffer = new ConcurrentDictionary<BufferKey, IEnumerable<T>>();

    /// <summary>
    /// Получить значение из буфера
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEnumerable<T>? Get(BufferKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        _buffer.TryGetValue(key, out IEnumerable<T>? value);
        return value;
    }

    /// <summary>
    /// Получить значение из буфера асинхронно.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>?> GetAsync(BufferKey key, CancellationToken token)
        => await Task.Run( () => Get(key), token);

    /// <summary>
    /// Сохранить данные в буфере. 
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="values"> Набор значений. </param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public void Save(BufferKey key, IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(values);
        _buffer.AddOrUpdate(key, values, (key, oldValues) => values );
    }

    /// <summary>
    /// Ассинхронный вариант записи данных в буфер.
    /// </summary>
    /// <param name="key"> Ключ <see cref="BufferKey"/> </param>
    /// <param name="values"> Набор значений. </param>
    /// <param name="token"> Токен отмены. </param>
    /// <returns></returns>
    public async Task SaveAsync(BufferKey key, IEnumerable<T> values, CancellationToken token)
        => await Task.Run( () => Save( key, values), token);
}
