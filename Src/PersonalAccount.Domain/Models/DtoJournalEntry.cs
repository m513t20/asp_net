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
    [DatabaseName("receiptn", typeof(string), Enums.DatabaseTypes.MSSql)]
    public string ReceiptNumber { get; set; }

    /// <summary>
    /// Уникальный код сотрудника.
    /// </summary>
    [DatabaseName("id", typeof(long), Enums.DatabaseTypes.MSSql)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// Уникальный код номенклатуры.
    /// </summary>
    [DatabaseName("id", typeof(long), Enums.DatabaseTypes.MSSql)]
    public long NomenclatureId { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    [StringLength(255)]
    [DatabaseName("description", typeof(string), Enums.DatabaseTypes.MSSql)]
    public string Description { get; set; }

    /// <summary>
    /// Уникальный код категории.
    /// </summary>
    [DatabaseName("categoryid", typeof(int), Enums.DatabaseTypes.MSSql)]
    public int CategoryId { get; set; }

    /// <summary>
    /// Уникальный код операции.
    /// </summary>
    [Required]
    [DatabaseName("transtype", typeof(long), Enums.DatabaseTypes.MSSql)]
    public long TransactionId { get; set; }

    /// <summary>
    /// Дата транзакции с временной зоной.
    /// </summary>
    [DatabaseName("dater", typeof(DateTimeOffset), Enums.DatabaseTypes.MSSql)]
    public DateTimeOffset TransactionDate { get; set; }

    /// <summary>
    /// Количество.
    /// </summary>
    [Required]
    [DatabaseName("quantity", typeof(int), Enums.DatabaseTypes.MSSql)]
    public int Amount { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    [Required]
    [DatabaseName("price", typeof(int), Enums.DatabaseTypes.MSSql)]
    public int Total { get; set; }

    /// <summary>
    /// Сумма скидки.
    /// </summary>
    [DatabaseName("discountgroup", typeof(uint), Enums.DatabaseTypes.MSSql)]
    public uint Discount { get; set; }
}
