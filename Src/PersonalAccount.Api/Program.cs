using System.Reflection;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PersonalAccount.Console.Models;
using PersonalAccount.Data;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var options = configuration.Get<ApplicationOptions>()
            ?? throw new InvalidOperationException("Unable to load appsettings.json");

// Подключаем миграцию
var upgrader =  DeployChanges.To
            .PostgresqlDatabase(options.ConnectionString)
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


using var connect = new NpgsqlConnection(options.ConnectionString);
connect.Open();

// using var transaction = connect.BeginTransaction();

// var dropCommand = new NpgsqlCommand(SqlScriptLoader.DropScript, connect);
// dropCommand.Transaction = transaction;
// dropCommand.ExecuteNonQuery();

// var createCommand = new NpgsqlCommand(SqlScriptLoader.CreateScript, connect);
// createCommand.Transaction = transaction;
// createCommand.ExecuteNonQuery();

// var fillCommand = new NpgsqlCommand(SqlScriptLoader.FillScript, connect);
// fillCommand.Transaction = transaction;
// fillCommand.ExecuteNonQuery();

// transaction.Commit();

// проверка загрузки
using var command = new NpgsqlCommand("SELECT * FROM organisations", connect);
using var reader = command.ExecuteReader();
while (reader.Read())
{
    // Access data by column index or name
    Guid id = reader.GetGuid(0); // Using column index
    var name = reader.GetString(1);
    var adress = reader.GetString(2);

    // Process the data
    Console.WriteLine($"ID: {id}, Name: {name}, Quantity: {adress}");
}