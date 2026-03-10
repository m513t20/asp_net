using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models.Enums;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель - транзакция.
/// </summary>
public class Transaction : IId<long>
{
    /// <summary>
    /// Уникальный код операции.
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Тип транзакции.
    /// </summary>
    public TransactionType Type {get; set;}

    /// <summary>
    /// Обслуживший сотрудник.
    /// </summary>
    public Employee ServedBy { get; set; }

    /// <summary>
    /// Организация, в которой обслуживали.
    /// </summary>
    public Organisation ServedIn { get; set; }

    /// <summary>
    /// Дата и время открытия чека.
    /// </summary>
    [Required]
    public DateTimeOffset OpeningTime { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Дата и время закрытия чека.
    /// </summary>
    public DateTimeOffset ClosingTime { get; set; }

    /// <summary>
    /// Используемая номенклатура.
    /// </summary>
    public Nomenclature UsedNomenclature { get; set; }

    /// <summary>
    /// Количество.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Сумма скидки.
    /// </summary>
    public uint Discount { get; set; }
}
