using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PersonalAccount.Data.Models;

namespace PersonalAccount.UnitTests;


/// <summary>
/// Набор тестов для проверки работы контекста базы данных.
/// </summary>
public class DbContextTests
{
    [Test]
    public async Task FetchCompanies_PersonalAccountContext_Any()
    {
        // Подготовка
        var context = new PersonalAccountContext();

        // Действие
        var result = await context.Organisations.ToListAsync(CancellationToken.None);

        // Проверка
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Any());
    }
}
