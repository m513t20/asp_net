using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

// Лого
CurrentApplication.ShowLogo();
                 
// Логгер
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                path: "PersonalAccount.Console.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30
            )
            .CreateLogger();

// Настройки приложения и сервисы
var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

var host = Host.CreateDefaultBuilder()
            .ConfigureServices( (context, services) =>
            {
                services.RegistryPersonalAccountConsole( configuration);
            });

await host.StartAsync();