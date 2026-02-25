using System;
using PersonalAccount.Domain.Models.Enums;

namespace PersonalAccount.Domain.Core.Attributes;

/// <summary>
/// атрибут для перевода из базы данных.
/// </summary>
public class DatabaseNameAttribute : Attribute
{
    /// <summary>
    /// Шаблон для проверки адреса.
    /// </summary>
    public string Name { get; set; }

    public Type DataType { get; set; }

    public DatabaseTypes DBType { get; set; }

    /// <summary>
    /// Создать инстанс класса <see cref="DatabaseNameAttribute">
    /// </summary>
    public DatabaseNameAttribute(string name, Type dataType, DatabaseTypes dbType)
    {
        Name = name;
        DataType = dataType;
        DBType = dbType;
    }
}
