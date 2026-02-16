using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PersonalAccount.Domain.Core.Attributes;

/// <summary>
/// Аттрибут для фиксации шаблона адреса.
/// </summary>
public class AdressTemplateAttribute : ValidationAttribute
{
    /// <summary>
    /// Шаблон для проверки телефонного номера.
    /// </summary>
    public string Template { get; set; } = """^\d{2}\d{3}\d{3}\d{3}\d{4}\d{4}$""";

    /// <summary>
    /// Создать инстанс класса <see cref="AdressTemplateAttribute">
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public AdressTemplateAttribute()
    {  }

    /// <summary>
    /// Валидация аттрибута Кладр - 19 цифр.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var match = new Regex(Template);
        if (
            value is not string stringValue
            || stringValue == null
            || !match.IsMatch(stringValue)
        )
        {
            return new ValidationResult(ErrorMessage ?? $"The field {validationContext.DisplayName} doesn't match with {Template} template");
        }

        return ValidationResult.Success;
    }
}
