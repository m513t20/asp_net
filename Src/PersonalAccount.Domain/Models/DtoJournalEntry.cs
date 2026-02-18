using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель Dto - запись в журнале.
/// </summary>
public class DtoJournalEntry : IId<long>
{
    /// <summary>
    /// Уникальный код.
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Уникальный номер чека.
    /// </summary>
    [Required]
    [StringLength(20)]
    public string ReceiptNumber { get; set; }

    /// <summary>
    /// Уникальный код сотрудника.
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// Уникальный код номенклатуры.
    /// </summary>
    public long NomenclatureId { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    [StringLength(255)]
    public string Description { get; set; }

    /// <summary>
    /// Уникальный код категории.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Уникальный код операции.
    /// </summary>
    [Required]
    public long TransactionId { get; set; }

    /// <summary>
    /// Дата транзакции с временной зоной.
    /// </summary>
    public DateTimeOffset TransactionDate { get; set; }

    /// <summary>
    /// Количество.
    /// </summary>
    [Required]
    public int Amount { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    [Required]
    public int Total { get; set; }

    /// <summary>
    /// Сумма скидки.
    /// </summary>
    public uint Discount { get; set; }
}
