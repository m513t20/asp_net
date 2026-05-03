using System;
using NUnit.Framework;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Logics;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.UnitTests;

/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


/// <summary>
/// Набор модульных тестов для проверки работы с буфером.
/// </summary>
public class BufferTests
{
    /// <summary>
    /// Добавить новую запись в буфер и получить значение.
    /// </summary>
    [Test]
    public void Save_AddNewRecord_DoesNotThrow()
    {
        // Подготовка
        var settings = new LoadingSettingsModel()
        {
            StartPosition = 1, BatchSize = 1000
        };
        var key = new BufferKey( settings, typeof(CategoryModel));
        var values = new List<CategoryModel>()
        {
            new CategoryModel()
            {
                Id = Guid.NewGuid()
            },
            new CategoryModel()
            {
                Id = Guid.NewGuid()
            }
        };
        var buffer = new Buffer<CategoryModel>();

        // Действие и проверки
        Assert.DoesNotThrow( () =>
        {
            buffer.Save( key, values);
            var items = buffer.Get( key );

            Assert.That( items?.Count() == values.Count());
        });
    }
}
