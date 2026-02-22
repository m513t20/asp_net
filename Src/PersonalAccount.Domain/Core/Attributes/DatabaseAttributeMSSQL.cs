using System;
using PersonalAccount.Domain.Core.Attributes.Abstract;

namespace PersonalAccount.Domain.Core.Attributes;

/// <summary>
/// Аттрибут для перевода данных таблицы типа mssql в модели.
/// </summary>
public class DatabaseAttributeMSSQL : DatabaseNameAttribute
{
    public DatabaseAttributeMSSQL(string name, Type dataType) : base(name, dataType)
    { }
}
