using System;
using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Console.Logics;

/// <summary>
/// Фоновый процесс для загрузки данных.
/// </summary>
public class BackgroundPushServer : BackgroundService
{
    private ConsoleOptions _options;

    private IClientRepository<JournalRowDto> _repo;

    public BackgroundPushServer(
        ConsoleOptions options,
         IClientRepository<JournalRowDto> repo
    )
    {
        _options = options;
        _repo = repo;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var client = new HttpClient();

            // Получаем текущие настройки для организации
            var url =  $"{_options.ServerHost}/console/{_options.CompanyId}" ;

            // получаем текущие настройки
            var response = await client.GetAsync( url );
            var settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>();
            if (settings is null) throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");

            // Загружаем первую пачку
            using var connect = new SqlConnection(_options.ConnectionString);
            var transactions = await _repo.GetRows( connect, settings );

            // Передаем данные пока получаем транзакции
            while (transactions.Count() > 0)
            {
                // 1. Передали транзакции
                await client.PostAsJsonAsync( url, transactions );
                
                // 2. Запросили новые настройки
                response = await client.GetAsync( url );
                settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>();
                if (settings is null) throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");

                // 3. Загрузили новую пачку
                transactions = await _repo.GetRows( connect, settings );           
            }
        }
    }
}
