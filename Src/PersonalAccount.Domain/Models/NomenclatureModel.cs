using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель номенклатуры.
/// </summary>
public class NomenclatureModel : DomainModel, IDto
{
    /// <summary>
    /// Наименование,
    /// </summary>
    [Required]
    public string Name {get;set;} = string.Empty;

    /// <summary>
    /// Категория.
    /// </summary>
    [Required]
    public CategoryModel Category {get;set;} = null!;
}
