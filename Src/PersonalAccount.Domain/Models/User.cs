using System;
using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель пользователя.
/// </summary>
public class User : IId<Guid>
{
    /// <summary>
    /// Уникальный код пользователя.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;
}
