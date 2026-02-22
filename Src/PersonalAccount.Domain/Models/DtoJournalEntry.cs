using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core.Attributes;
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
    [DatabaseAttributeMSSQL("receiptn", typeof(string))]
    public string ReceiptNumber { get; set; }

    /// <summary>
    /// Уникальный код сотрудника.
    /// </summary>
    [DatabaseAttributeMSSQL("id", typeof(long))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// Уникальный код номенклатуры.
    /// </summary>
    [DatabaseAttributeMSSQL("id", typeof(long))]
    public long NomenclatureId { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    [StringLength(255)]
    [DatabaseAttributeMSSQL("description", typeof(string))]
    public string Description { get; set; }

    /// <summary>
    /// Уникальный код категории.
    /// </summary>
    [DatabaseAttributeMSSQL("categoryid", typeof(int))]
    public int CategoryId { get; set; }

    /// <summary>
    /// Уникальный код операции.
    /// </summary>
    [Required]
    [DatabaseAttributeMSSQL("transtype", typeof(long))]
    public long TransactionId { get; set; }

    /// <summary>
    /// Дата транзакции с временной зоной.
    /// </summary>
    [DatabaseAttributeMSSQL("dater", typeof(DateTimeOffset))]
    public DateTimeOffset TransactionDate { get; set; }

    /// <summary>
    /// Количество.
    /// </summary>
    [Required]
    [DatabaseAttributeMSSQL("quantity", typeof(int))]
    public int Amount { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    [Required]
    [DatabaseAttributeMSSQL("price", typeof(int))]
    public int Total { get; set; }

    /// <summary>
    /// Сумма скидки.
    /// </summary>
    [DatabaseAttributeMSSQL("discountgroup", typeof(uint))]
    public uint Discount { get; set; }
}
