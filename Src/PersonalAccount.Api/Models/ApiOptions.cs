using System;

namespace PersonalAccount.Api.Models;

/// <summary>
/// Настройки приложения.
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Строка подключения MS SQL
    /// </summary>
    public required string MsSqlConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Строка подключения PgSql
    /// </summary>
    public required string PgSqlConnectionString { get; set; } = string.Empty;
}
