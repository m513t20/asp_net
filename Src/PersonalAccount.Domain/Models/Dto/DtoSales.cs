using System;

namespace PersonalAccount.Domain.Models.Dto;

/// <summary>
/// Модель Dto для учета выручки.
/// </summary>
public class DtoSales
{
    /// <summary>
    /// Код группы.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Название группы.
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// Код номенклатуры. 
    /// </summary>
    public long NomenclatureId { get; set; }

    /// <summary>
    /// Название номенклатуры,
    /// </summary>
    public string NomenclatureName { get; set; } = string.Empty;

    /// <summary>
    /// Количество.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Сумма. 
    /// </summary>
    public double Cost { get; set; }

    /// <summary>
    /// Скидка.
    /// </summary>
    public double Discount { get; set; }

    /// <summary>
    /// Код организации.
    /// </summary>
    public Guid OrganisationId { get; set; }
}
