using System;

namespace PersonalAccount.Domain.Models.Dto;

/// <summary>
/// Модель Dto для учета выручки.
/// </summary>
public class DtoProfit
{
// | Период | Сумма оплаты наличные | Сумма оплаты безналично | Сумма оплаты остальное | Сумма скидки | Флаг "Праздник" | Код организации |
    /// <summary>
    /// Начало периода.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// Конец периода.
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Сумма наличные.
    /// </summary>
    public double CashProfit { get; set; }

    /// <summary>
    /// Сумма безналично.
    /// </summary>
    public double CardProfit { get; set; }

    /// <summary>
    /// Сумма остальное.
    /// </summary>
    public double OtherProfit { get; set; }

    /// <summary>
    /// Скидка.
    /// </summary>
    public double Discount { get; set; }

    /// <summary>
    /// Флаг праздника
    /// </summary>
    public bool IsHoliday { get; set; }

    /// <summary>
    /// Код организации.
    /// </summary>
    public Guid OrganisationId { get; set; }
}
