using System;

namespace PersonalAccount.Domain.Core;

/// <summary>
/// Аттррибут для фиксации шаблона телефона.
/// </summary>
public class PhoneTemplateAttribute : Attribute
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
}
