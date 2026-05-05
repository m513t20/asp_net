using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Фоновый процесс отправки данных.
/// </summary>
public class JournalBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Guid _targetCompanyId = Guid.Parse("14e54725-0efc-42b8-a27d-a84f9a7257c5");

    public JournalBackgroundService(
        IServiceScopeFactory scopeFactory
    )
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Запуск загрузки данных.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            
            var dataSource = scope.ServiceProvider.GetRequiredService<IJournalDataSource>();
            var extractor = scope.ServiceProvider.GetRequiredService<IEntityExtractor>();
            var repository = scope.ServiceProvider.GetRequiredService<IBusinessDataRepository>();
            var loadingService = scope.ServiceProvider.GetRequiredService<ILoadingService>(); //

            var settings = await loadingService.GetSettingsAsync(_targetCompanyId, stoppingToken);

            var branch = new BranchModel { Id = _targetCompanyId };

            var rawRows = await dataSource.GetUnprocessedRowsAsync(branch, (int)settings.BatchSize, stoppingToken);
            var rawRowsList = rawRows.ToList();

            if (rawRowsList.Any())
            {
                var newCategories = await extractor.ExtractNewCategoriesAsync(rawRowsList, stoppingToken);
                var newNomenclature = await extractor.ExtractNewNomenclatureAsync(rawRowsList, stoppingToken);
                var newEmployees = await extractor.ExtractNewEmployeesAsync(rawRowsList, stoppingToken);

                await repository.SaveCategoriesAsync(newCategories, stoppingToken);
                await repository.SaveNomenclatureAsync(newNomenclature, stoppingToken);
                await repository.SaveEmployeesAsync(newEmployees, stoppingToken);

                var transactions = rawRowsList.Select(x => new TransactionModel
                {
                    Id = Guid.NewGuid(),
                    Type = (TransactionType)x.TypeCode,
                    Quantuty = x.Quantity,
                    Price = x.Price,
                    Discount = x.Discount,
                    Period = x.Period,
                    Owner = new CompanyModel { Id = _targetCompanyId },
                    Emploee = x.EmploeeCode.HasValue ? new EmploeeModel { Code = x.EmploeeCode.Value } : null,
                    Nomenclature = x.ProductCode.HasValue ? new NomenclatureModel { Code = x.ProductCode.Value } : null
                }).ToList();
                await repository.SaveTransactionsAsync(transactions, stoppingToken);

                var lastCode = rawRowsList.Max(x => x.Code);
                settings.StartPosition = lastCode;
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
