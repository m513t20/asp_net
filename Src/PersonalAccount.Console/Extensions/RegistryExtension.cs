using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Console.Logics;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Console.Extensions;

/// <summary>
/// Регистрация сервисов модуля в DI
/// </summary>
public static class RegistryExtension
{
    /// <summary>
    /// Зарегистрировать в контейнере сервисы модуля PersonalAccount.Data
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegistryPersonalAccountConsole
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var options = configuration.GetSection(nameof(ConsoleOptions)).Get<ConsoleOptions>()
                        ?? throw new InvalidOperationException($"Невозможно загрузить настройки из секции {nameof(ConsoleOptions)}!");

        services.AddScoped<  IClientRepository<JournalRowDto> , JournalReadRepository >();
        services.AddSingleton( x => options );
        services.AddHostedService<BackgroungPushService>();
        
        return services;
    }
}
