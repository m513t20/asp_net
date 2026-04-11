using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAccount.Api;
using PersonalAccount.Console.Models;
using PersonalAccount.Data.Logics;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Utils;

namespace PersonalAccount.Console.Extensions;

/// <summary>
/// Регистрация сервисов модуля в DI.
/// </summary>
public static class RegistryPersonalAccounApi
{
    /// <summary>
    /// Зарегистрировать в констексте сервисы и модули PersonalAccount.Api
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegistrytPersonalAccountApi
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<ILoadSettingsRepository, LoadingSettingsRepository>();
        services.AddSingleton<LoadingSevice>();

        var options = configuration.Get<ConsoleOptions>()
                    ?? throw new InvalidOperationException("Unable to load appsettings.json");
        var connectionString = options.MsSqlConnectionString; 
        services.AddDbContext<PersonalAccountContext>(
            x => x.UseNpgsql( connectionString )
        );

        services.AddScoped(
            x => new SqlConnection(options.MsSqlConnectionString)
        );

        return services;
    }

}
