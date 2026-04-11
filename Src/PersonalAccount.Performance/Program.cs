using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PersonalAccount.Console.Models;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var options = configuration.Get<ConsoleOptions>()
            ?? throw new InvalidOperationException("Unable to load appsettings.json");

using var connect = new SqlConnection(options.MsSqlConnectionString);
connect.Open();

// Смотрим по 2023 году

Stopwatch stopWatch = new Stopwatch();

// День
stopWatch.Start();
var sql = "select * from journal where  CAST(dater AS DATE) = '2023-02-01'";
var command = new SqlCommand(sql, connect);
var adapter = new SqlDataAdapter(command);
var dataset = new DataSet();
adapter.Fill(dataset);
stopWatch.Stop();

var ts = stopWatch.Elapsed;
Console.WriteLine($"One day operation ran in {ts.TotalMilliseconds} ms");

// месяц
stopWatch.Restart();
sql = "select * from journal where  CAST(dater AS DATE)  >= '2023-02-01' AND  CAST(dater AS DATE)  < '2023-03-01'";
command = new SqlCommand(sql, connect);
adapter = new SqlDataAdapter(command);
dataset = new DataSet();
adapter.Fill(dataset);
stopWatch.Stop();

ts = stopWatch.Elapsed;
Console.WriteLine($"One month operation ran in {ts.TotalMilliseconds} ms");

// Квартал
stopWatch.Restart();
sql = "select * from journal where  CAST(dater AS DATE)  >= '2023-03-01' AND  CAST(dater AS DATE)  < '2023-06-01'";
command = new SqlCommand(sql, connect);
adapter = new SqlDataAdapter(command);
dataset = new DataSet();
adapter.Fill(dataset);
stopWatch.Stop();

ts = stopWatch.Elapsed;
Console.WriteLine($"One quarter operation ran in {ts.TotalMilliseconds} ms");