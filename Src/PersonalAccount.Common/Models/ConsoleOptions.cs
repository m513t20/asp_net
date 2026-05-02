using System;

namespace PersonalAccount.Common.Models;

/// <summary>
/// Настройки для Console
/// </summary>
public class ConsoleOptions
{
    /// <summary>
    /// Строка подключения к MSSQL
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Хост для подключения к серверу
    /// </summary>
    public string ServerHost { get; set; } = null!;

    /// <summary>
    /// Уникальный код подразделения
    /// </summary>
    public Guid BranchId { get; set; }
}
