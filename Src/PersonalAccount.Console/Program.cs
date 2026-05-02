using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Domain;
using Serilog;

// Лого
CurrentApplication.ShowLogo();

// Логгер
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    path: "PersonalAccountConsole_.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30
                )
                .CreateLogger();                 

// Настройки приложения и сервисы
var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

// Создаем хост
try
{
    var host = Host.CreateDefaultBuilder()
        .UseSerilog() 
        .ConfigureServices((context, services) =>
        {
            services.RegistryPersonalAccountConsole(configuration);
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex) 
{
    Log.Fatal(ex, $"Хост завершился с ошибкой!\n{ex.Message}{ex.InnerException?.Message}");
}
finally
{
    Log.CloseAndFlush();
}
   

          