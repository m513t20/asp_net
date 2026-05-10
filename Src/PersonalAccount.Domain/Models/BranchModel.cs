using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель структурного подразделения организации.
/// </summary>
public class BranchModel : DomainModel
{
    /// <summary>
    /// Наименование подразделения.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Организация владелец подразделения.
    /// </summary>
    public CompanyModel Owner { get;set; } = null!;

    /// <summary>
    /// Текущие настройеи загрузки данных.
    /// </summary>
    public LoadingSettingsModel Settings { get; set; } = null!;
}
