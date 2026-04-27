using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель категории номераклатуры.
/// </summary>
public class CategoryModel : DomainModel, IDto
{
    /// <summary>
    /// Наименование категории.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Организация владелец категории.
    /// </summary>
    [Required]
    public CompanyModel Owner {get;set;} = null!;
}
