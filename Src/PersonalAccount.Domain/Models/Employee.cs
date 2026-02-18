using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core.Attributes;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель - сотрудник.
/// </summary>
public class Employee : IId<long>, IName
{
    /// <summary>
    /// Уникальный код сотрудника.
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Наименование сотрудника.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Контактный телефон.
    /// </summary>
    [PhoneTemplate("^[0-9]{10,11}$")]
    public string Phone { get; set; }

    /// <summary>
    /// Организация работодатель.
    /// </summary>
    [Required]
    public Organisation WorkOrganisation { get; set; }
}
