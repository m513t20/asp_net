using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using PersonalAccount.Domain.Core.Attributes;

namespace PersonalAccount.Domain.Core;

/// <summary>
/// Класс для валидации моделей.
/// </summary>
public class PropertyValidator
{
    /// <summary>
    /// Метод для валидации всех полей модели по их аттрибутам.
    /// </summary>
    /// <param name="model">Экземпляр модели.</param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public static bool ValidateModel(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, context, results, true);
    }
}
