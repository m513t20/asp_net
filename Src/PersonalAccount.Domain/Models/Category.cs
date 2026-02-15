using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель категории номенклатуры.
/// </summary>
public class Category : IId<int>, IName
{
    /// <summary>
    /// Уникальный код категории.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Наименование категории.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Организация, которая использует номенклатуру.
    /// </summary>
    public Organisation UserOrganisation { get; set; }
}
