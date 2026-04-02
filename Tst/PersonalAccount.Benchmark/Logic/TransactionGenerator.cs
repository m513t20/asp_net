using System;
using System.Reflection;
using System.Text.Json;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Benchmark.Logic;

public class TransactionGenerator
{
    public static List<Transaction> GenerateTransactions(int amount)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream($"PersonalAccount.Benchmark.Data.transactions.json")
            ?? throw new ArgumentNullException($"couldn't find fitting assembly for transactions.json");
        using StreamReader reader = new StreamReader(stream);
        var text = reader.ReadToEnd();

        var transactions =  JsonSerializer.Deserialize<List<Transaction>>(text)
            ?? throw new ArgumentNullException("loaded transactions is null");

        var result = Enumerable.Repeat(transactions, amount / transactions.Count)
            .SelectMany(x => x)
            .ToList();
        return result;
    }
}
