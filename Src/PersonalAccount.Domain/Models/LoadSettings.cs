using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models.Enums;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель - настройки загрузки.
/// </summary>
public class LoadSettings : IId<Guid>
{
    /// <summary>
    /// Уникальный код.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Размер пачки для загрузки.
    /// </summary>
    [Required]
    public int PackSize { get; set; }

    /// <summary>
    /// Тип принимаемых данных.
    /// </summary>
    [Required]
    public LoadDataTypes LoadDataType { get; set; }

    /// <summary>
    /// Тип принимаемых данных.
    /// </summary>
    [Required]
    public Organisation UserOrganisation { get; set; }
}
