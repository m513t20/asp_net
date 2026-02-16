using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Attributes;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.UnitTests;

/// <summary>
/// 
/// </summary>
public class ApplicationTests
{
    /// <summary>
    /// Проверяем создание категории с именование != null
    /// </summary>
    [Test]
    public void Create_CheckNullName()
    {
        // Подготовка
        var domain = new Category();

        // Действие

        // Проверка
        Assert.That (domain.Name is not null);
    }

    [Test]
    public void Create_Category_ExistsAttributes()
    {
        // Подготовка
        var domain = new Category();
        var type = typeof(Category);

        // Действие
        var properties = type.GetProperties().Where(x => x.GetCustomAttributes(true).Any());

        // Проверка
        Assert.That(properties.Any());
    }

    /// <summary>
    /// Проверь наличие атрибута <see cref="PhoneTemplateAttribute"/> 
    /// </summary>
    [Test]
    public void Create_Employee_ExistsPhoneTemplateAttribute()
    {
        // Подготовка
        var domain = new Employee();

        // Действие
        var properties = domain.GetType()
                            .GetProperties()
                            .Where(x => x.GetCustomAttribute<PhoneTemplateAttribute>(true) is not null);

        // Проверки
        Assert.That(properties.Any());
        var attribute  = properties.First().GetCustomAttribute<PhoneTemplateAttribute>();
        Assert.That(!string.IsNullOrEmpty(attribute!.Template));

        var match = new Regex(attribute!.Template);
        Assert.Throws<ArgumentNullException>(() => match.IsMatch(domain.Phone!));
    }
}
