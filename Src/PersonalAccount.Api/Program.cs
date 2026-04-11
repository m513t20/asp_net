using System.Reflection;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Console.Models;
using PersonalAccount.Data;


// Настройки
var builder = WebApplication.CreateBuilder();
var configuration = new ConfigurationBuilder()
                    // .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
var options = configuration.Get<ConsoleOptions>()
            ?? throw new InvalidOperationException("Unable to load appsettings.json");

// Подключаем миграцию
var upgrader =  DeployChanges.To
            .PostgresqlDatabase(options.PgSqlConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(PersonalAccount.Data.PersonalAccountDataMarker)))
            .LogToConsole()
            .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
}

// Сервисы
builder.Services
        .RegistrytPersonalAccountData( configuration )
        .RegistrytPersonalAccountApi( configuration );


// API
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://0.0.0.0:8000");
var application = builder.Build();
application.UseDeveloperExceptionPage();
application.UseRouting();
application.MapControllers();
application.Run();

// using var connect = new NpgsqlConnection(options.ConnectionString);
// connect.Open();

// // using var transaction = connect.BeginTransaction();

// // var dropCommand = new NpgsqlCommand(SqlScriptLoader.DropScript, connect);
// // dropCommand.Transaction = transaction;
// // dropCommand.ExecuteNonQuery();

// // var createCommand = new NpgsqlCommand(SqlScriptLoader.CreateScript, connect);
// // createCommand.Transaction = transaction;
// // createCommand.ExecuteNonQuery();

// // var fillCommand = new NpgsqlCommand(SqlScriptLoader.FillScript, connect);
// // fillCommand.Transaction = transaction;
// // fillCommand.ExecuteNonQuery();

// // transaction.Commit();

// // проверка загрузки
// using var command = new NpgsqlCommand("SELECT * FROM organisations", connect);
// using var reader = command.ExecuteReader();
// while (reader.Read())
// {
//     // Access data by column index or name
//     Guid id = reader.GetGuid(0); // Using column index
//     var name = reader.GetString(1);
//     var adress = reader.GetString(2);

//     // Process the data
//     Console.WriteLine($"ID: {id}, Name: {name}, Quantity: {adress}");
// }