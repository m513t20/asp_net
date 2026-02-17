using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.UnitTests;

public class ValidationTests
{
    /// <summary>
    /// Проверка валидации атрибута <see cref="PhoneTemplateAttribute"/> 
    /// </summary>
    [Test]
    public void Validate_PhoneAttribute()
    {
        // Подготовка
        var employee = new Employee(){ Name = "Joe", Phone = "88005553535", WorkOrganisation = new() };

        // Действие
        bool result = PropertyValidator.ValidateModel(employee);

        // Проверка
        Assert.That(result == true);
    }

    /// <summary>
    /// Проверка валидации атрибута <see cref="PhoneTemplateAttribute"/> с пустым полем 
    /// </summary>
    [Test]
    public void Validate_NullPhoneAttribute()
    {
        // Подготовка
        var employee = new Employee();

        // Действие
        bool result = PropertyValidator.ValidateModel(employee);

        // Проверка
        Assert.That(result == false);
    }

    /// <summary>
    /// Проверка валидации атрибута <see cref="PhoneTemplateAttribute"/> с неверно заполненным полем 
    /// </summary>
    [Test]
    public void Validate_WrongPhoneAttribute()
    {
        // Подготовка
        var employee = new Employee(){ Phone = "8800555353sad5" };

        // Действие
        bool result = PropertyValidator.ValidateModel(employee);

        // Проверка
        Assert.That(result == false);
    }


    /// <summary>
    /// Проверка валидации атрибута <see cref="AdressTemplateAttribute"/> 
    /// </summary>
    [Test]
    public void Validate_AdressAttribute()
    {
        // Подготовка
        var organisation = new Organisation(){ Name = "OOO RGD", Settings = new(), Adress = "обл Иркутская, р-н Шелеховский, с Баклаши, ул Иркутская, дом 1, кв 1" };

        // Действие
        bool result = PropertyValidator.ValidateModel(organisation);

        // Проверка
        Assert.That(result == true);
    }

    /// <summary>
    /// Проверка валидации атрибута <see cref="AdressTemplateAttribute"/> с неверно заполненным полем 
    /// </summary>
    [Test]
    public void Validate_WrongAdressAttribute()
    {
        // Подготовка
        var organisation = new Organisation(){ Name = "OOO RGD", Settings = new(), Adress = "с Баклаши, ул Иркутская, дом 1, кв 1" };

        // Действие
        bool result = PropertyValidator.ValidateModel(organisation);

        // Проверка
        Assert.That(result == false);
    }

}
