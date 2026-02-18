using System;

namespace PersonalAccount.Domain.Core.Interfaces;

/// <summary>
/// Общий интерфейс для работы с моделью.
/// </summary>
/// <typeparam name="TIdType">Тип данных для идентификатора.</typeparam>
public interface IId<TIdType>
{
    /// <summary>
    /// Уникальный код
    /// </summary>
    public TIdType Id {get; set;}
}
