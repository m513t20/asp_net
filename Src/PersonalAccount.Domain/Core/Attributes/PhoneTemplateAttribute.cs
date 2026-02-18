using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PersonalAccount.Domain.Core.Attributes;

/// <summary>
/// Аттрибут для фиксации шаблона телефона.
/// </summary>
public class PhoneTemplateAttribute : ValidationAttribute
{
    /// <summary>
    /// Шаблон для проверки телефонного номера.
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// Создать инстанс класса <see cref="PhoneTemplateAttribute">
    /// </summary>
    /// <param name="template">Шаблон для регулярного выражения. </param>
    /// <exception cref="ArgumentNullException"></exception>
    public PhoneTemplateAttribute(string template)
    {
        Template = template ?? throw new ArgumentNullException(nameof(template));
    }

    /// <summary>
    /// Валидация аттрибута.
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
