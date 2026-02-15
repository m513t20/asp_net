using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель - номенклатура.
/// </summary>
public class Nomenclature : IId<long>, IName
{
    /// <summary>
    /// Уникальный код номенклатуры.
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Категория, которой принадлежит номенклатура.
    /// </summary>
    [Required]
    public Category ParentCategory { get; set; }

    /// <summary>
    /// Единицы измерения.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string MeasureUnit { get; set; } = string.Empty;

    /// <summary>
    /// Наименование номенклатуры.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
}
