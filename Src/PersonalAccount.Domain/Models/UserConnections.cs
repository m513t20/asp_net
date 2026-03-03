using System.ComponentModel.DataAnnotations;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models;

/// <summary>
/// Модель связи пользователей и организаций.
/// </summary>
public record class UserConnections : IId<Guid>
{
    /// <summary>
    /// Уникальный код пользователя.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Модель организации.
    /// </summary>
    [Required]
    public Organisation OrganisationModel { get; set; }

    /// <summary>
    /// Модель пользователя
    /// </summary>
    [Required]
    public User UserModel { get; set; }
}
