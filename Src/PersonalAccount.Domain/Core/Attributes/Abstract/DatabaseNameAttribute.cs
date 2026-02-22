using System;

namespace PersonalAccount.Domain.Core.Attributes.Abstract;

/// <summary>
/// Абстрактный атрибут для перевода из базы данных.
/// </summary>
public class DatabaseNameAttribute : Attribute
{
    /// <summary>
    /// Шаблон для проверки адреса.
    /// </summary>
    public string Name { get; set; }

    public Type DataType { get; set; }

    /// <summary>
    /// Создать инстанс класса <see cref="DatabaseNameAttribute">
    /// </summary>
    public DatabaseNameAttribute(string name, Type dataType)
    {
        Name = name;
        DataType = dataType;
    }
}
