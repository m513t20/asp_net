using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Api;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Domain.Models.Dto;
namespace PersonalAccount.UnitTests;

public class PushService
{
    private readonly LoadingSevice _service;
    private ServiceProvider _provider;
    private IServiceScope _scope;

    public PushService()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var configuration = builder.Build();

        var services = new ServiceCollection()
                     .RegistrytPersonalAccountApi( configuration );

        _provider = services.BuildServiceProvider();        
        _scope = _provider.CreateScope();
        _service = _scope.ServiceProvider.GetRequiredService<LoadingSevice>();
    }
    
    [Test]
    public void Push_ReturnsTrue()
    {
        // Подготовка
        var organisation = new Domain.Models.Organisation { Id = new ("5eefff0e-b7dc-4c07-9b71-9115873bb8f3"), Settings = new() { Id = Guid.NewGuid(), PackSize = 100, LoadDataType = Domain.Models.Enums.LoadDataTypes.Json, StartPosition = 0}};
        var entries = new List<DtoJournalEntry>
        {
            new() { Id = 100, Amount = 500 }
        };

        // Действие
        var result = _service.Push(organisation, entries, CancellationToken.None);

        // Проверка
        Assert.That(result, Is.True);
    }
}
