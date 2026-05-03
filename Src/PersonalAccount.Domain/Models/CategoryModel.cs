using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель категории номераклатуры.
/// </summary>
public class CategoryModel : DomainModel
{
    /// <summary>
    /// Наименование категории.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Уникальный внешний код.
    /// </summary>
    public long? ExternalCode { get; set;}

    /// <summary>
    /// Организация владелец категории.
    /// </summary>
    [Required]
    public CompanyModel Owner {get;set;} = null!;
}
