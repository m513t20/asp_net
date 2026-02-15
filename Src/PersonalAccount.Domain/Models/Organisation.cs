using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель - организация.
/// </summary>
public class Organisation : IId<Guid>, IName
{
    /// <summary>
    /// Уникальный код.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Настройки загрузки данных.
    /// </summary>
    [Required]
    public LoadSettings Settings { get; set; }
    
    /// <summary>
    /// Наименование организации.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Адрес организации.
    /// </summary>
    // TODO: Параметр для валидации адреса
    [Required]
    [StringLength(255)]
    public string Adress { get; set; } = string.Empty;
}
