using System;
using NUnit.Framework;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Enums;
using PersonalAccount.Domain.Utils;

namespace PersonalAccount.UnitTests;

/// <summary>
/// Тестирование отчетов.
/// </summary>
public class ReportTests
{
    private List<Transaction> _testTransactions;
    private Organisation _organisation1;
    private Organisation _organisation2;
    private Employee _employee1;
    private Employee _employee2;
    private Category _categoryDrinks;
    private Category _categoryFood;
    private Nomenclature _coffee;
    private Nomenclature _tea;
    private Nomenclature _sandwich;

    [SetUp]
    public void Setup()
    {
        _organisation1 = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = "Кофейня у дома",
            Settings = new(),
            Adress = "ул. Ленина, 1"
        };

        _organisation2 = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = "Ресторан Националь",
            Settings = new(),
            Adress = "пр. Мира, 10"
        };

        // Сотрудники
        _employee1 = new Employee
        {
            Id = 101,
            Name = "Иван Петров",
            Phone = "89991234567",
            WorkOrganisation = _organisation1
        };

        _employee2 = new Employee
        {
            Id = 102,
            Name = "Мария Сидорова",
            Phone = "89997654321",
            WorkOrganisation = _organisation1
        };

        // Категории
        _categoryDrinks = new Category
        {
            Id = 1,
            Name = "Напитки"
        };

        _categoryFood = new Category
        {
            Id = 2,
            Name = "Еда"
        };

        // Номенклатура
        _coffee = new Nomenclature
        {
            Id = 1001,
            Name = "Кофе американо",
            ParentCategory = _categoryDrinks,
            MeasureUnit = "порц"
        };

        _tea = new Nomenclature
        {
            Id = 1002,
            Name = "Чай зеленый",
            ParentCategory = _categoryDrinks,
            MeasureUnit = "порц"
        };

        _sandwich = new Nomenclature
        {
            Id = 2001,
            Name = "Сэндвич с курицей",
            ParentCategory = _categoryFood,
            MeasureUnit = "шт"
        };


        _testTransactions = new List<Transaction>
        {
            
            new Transaction
            {
                Id = 1,
                Type = TransactionType.JobStart,
                ServedBy = _employee1,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 8, 0, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 8, 0, 0, TimeSpan.Zero),
                ServedIn = _organisation1
            },
            
            new Transaction
            {
                Id = 2,
                Type = TransactionType.JobFinish,
                ServedBy = _employee1,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 18, 0, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 18, 0, 0, TimeSpan.Zero),
                ServedIn = _organisation1
            },
            
            new Transaction
            {
                Id = 3,
                Type = TransactionType.Cash,
                ServedBy = _employee1,
                ServedIn = _organisation1,
                UsedNomenclature = _coffee,
                Amount = 2,
                Total = 150, // 150 руб за порцию
                Discount = 10,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 9, 15, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 9, 15, 0, TimeSpan.Zero)
            },
            
            new Transaction
            {
                Id = 4,
                Type = TransactionType.Visa,
                ServedBy = _employee1,
                ServedIn = _organisation1,
                UsedNomenclature = _tea,
                Amount = 1,
                Total = 120,
                Discount = 5,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 10, 30, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 10, 30, 0, TimeSpan.Zero)
            },
            
            new Transaction
            {
                Id = 5,
                Type = TransactionType.WriteOff,
                ServedBy = null,
                ServedIn = _organisation2,
                UsedNomenclature = _sandwich,
                Amount = 1,
                Total = 350,
                Discount = 20,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 13, 0, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 13, 0, 0, TimeSpan.Zero)
            },
            
            new Transaction
            {
                Id = 6,
                Type = TransactionType.PLUSale,
                ServedBy = _employee2,
                ServedIn = _organisation1,
                UsedNomenclature = _coffee,
                Amount = 1,
                Total = 150,
                Discount = 0,
                OpeningTime = new DateTimeOffset(2025, 3, 10, 14, 0, 0, TimeSpan.Zero),
                ClosingTime = new DateTimeOffset(2025, 3, 10, 14, 0, 0, TimeSpan.Zero)
            }
        };
    }

    [Test]
    public void GetJobReport_NotNull()
    {
        // Дейтсиве
        var result = ReportMaster.GetJobReport(_testTransactions);

        // Проверка
        Assert.That(result is not null);
    }

    [Test]
    public void GetSalesReport_NotNull()
    {
        // Действие
        var result = ReportMaster.GetSalesReport(_testTransactions);

        // Проверка
        Assert.That(result is not null);
    }

    [Test]
    public void GetProfitReport_NotNull()
    {
        // Действие
        var result = ReportMaster.GetProfitReport(_testTransactions);

        // Проверка
        Assert.That(result is not null);
    }
}
