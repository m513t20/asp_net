using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PersonalAccount.Benchmark.Logic;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Utils;

namespace PersonalAccount.Benchmark;

/// <summary>
/// Бенчмарк для создания отчетов.
/// </summary>
public class ReportCreationBecnhmark
{
    /// <summary>
    /// Набор 100 транзакций.
    /// </summary>
    private List<Transaction> _smallData = new();
    
    /// <summary>
    /// Набор 1000 транзакций.
    /// </summary>
    private List<Transaction> _mediumData = new();
    
    /// <summary>
    /// Набор 100 000 транзакций.
    /// </summary>
    private List<Transaction> _bigData = new();


    public ReportCreationBecnhmark()
    {
        _smallData = TransactionGenerator.GenerateTransactions(100);
        _mediumData = TransactionGenerator.GenerateTransactions(1000);
        _bigData = TransactionGenerator.GenerateTransactions(100000);
    }

    [Benchmark]
    public void ReportTransactionsSmall()
    {
        ReportMaster.GetProfitReport(_smallData);
    }
    
    [Benchmark]
    public void ReportTransactionsMediuml()
    {
        ReportMaster.GetProfitReport(_mediumData);
    }
    
    [Benchmark]
    public void ReportTransactionsBig()
    {
        ReportMaster.GetProfitReport(_bigData);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ReportCreationBecnhmark>();
    }
}