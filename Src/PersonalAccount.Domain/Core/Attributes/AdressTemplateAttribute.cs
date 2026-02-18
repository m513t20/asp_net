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
    /// Шаблон для проверки адреса.
    /// </summary>
    public string SplitChar { get; set; }

    /// <summary>
    /// Создать инстанс класса <see cref="AdressTemplateAttribute">
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public AdressTemplateAttribute(string splitChar)
    {
        SplitChar = splitChar;
    }

    /// <summary>
    /// Валидация аттрибута Кладр - 19 цифр.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var match = new Regex(SplitChar);
        if (
            value is not string stringValue
            || stringValue == null
            || match.Matches(stringValue).Count < 5
            || match.Matches(stringValue).Count > 6
        )
        {
            return new ValidationResult(ErrorMessage ?? $"The field {validationContext.DisplayName} doesn't match with {SplitChar} template");
        }

        return ValidationResult.Success;
    }
}
