using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Serilog;

namespace PersonalAccount.Console.Logics;

/// <summary>
/// Фоновый процесс для загрузки данных журнала
/// </summary>
public class BackgroungPushService : BackgroundService
{
    // Настройки
    private ConsoleOptions _options;

    // Репозиторий
    private IClientRepository<JournalRowDto> _repo;

    private readonly HttpClient _client;

    public BackgroungPushService(
            ConsoleOptions options,
            IClientRepository<JournalRowDto> repo)
    {
        _options = options;
        _repo = repo;

        // Отключаем Proxy т.к. работаем локально
        var handler = new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null,
        };

        // Добавляем базовые настройки
        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri(_options.ServerHost),
            Timeout = TimeSpan.FromSeconds(5)
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Фоновый сервис загрузки данных запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var url = $"{_options.ServerHost}/console/{_options.BranchId}";

            try
            {
                // Получить текущую настройки
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Хост {url} не доступен!\nHTTP статус: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");

                var settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>();
                if (settings is null) throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");

                // Загружаем пачку
                using var connect = new SqlConnection(_options.ConnectionString);
                var transactions = await _repo.GetRows(connect, settings);
                if(transactions.Any())
                    await _client.PostAsJsonAsync(url, transactions);
                    else
                {
                    Log.Information( string.Format("Для настройки: {0} нет данных.", JsonSerializer.Serialize( settings) ));
                    Thread.Sleep( 500 );
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Невозможно загрузить пачку данных!\n{ex.Message}{ex.InnerException?.Message}");
            }
        }
        Log.Information("Фоновый сервис загрузки данных остановлен.");
    }
}
