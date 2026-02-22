using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalAccount.Console.Models;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Core.Attributes;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Enums;
using PersonalAccount.Domain.Utils;

Console.WriteLine(LogoConf.GetLogo());

// var builder = new ConfigurationBuilder()
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json");

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var options = configuration.Get<ApplicationOptions>()
            ?? throw new InvalidOperationException("Unable to load appsettings.json");

using var connect = new SqlConnection(options.ConnectionString);
connect.Open();

var sql = "select Top 10 * from journal";
var command = new SqlCommand(sql, connect);

// Вариант 1
/*
var reader = command.ExecuteReader();
var position = 1;

while (reader.Read())
{
    Console.WriteLine($"{position} - {reader.GetInt32(0)} - {reader.GetInt32(4)}");
    position++;
}
*/

// Вариант 2
var adapter = new SqlDataAdapter(command);
var dataset = new DataSet();
adapter.Fill(dataset);

var table = dataset.Tables[0];

TableToModelConverter.ConvertToDto(table, DatabaseTypes.MSSql);

while (true)
{
    await Task.Delay(TimeSpan.FromHours(1));
}