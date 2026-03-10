using System;
using PersonalAccount.Domain.Core.Interfaces;

namespace PersonalAccount.Domain.Models.Dto;

/// <summary>
/// Модель Dto для учета графика работы.
/// </summary>
public class DtoJobSchedule : IDto
{
    /// <summary>
    /// Код сотрудника
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// ФИО.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Дата время начала.
    /// </summary>
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// Дата время окончания.
    /// </summary>
    public DateTimeOffset FinishTime { get; set; }

    /// <summary>
    /// Код организации.
    /// </summary>
    public Guid OrganisationId { get; set; }}
