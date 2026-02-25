using System.Data;
using System.Reflection;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Core.Attributes;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Enums;

namespace PersonalAccount.Domain.Utils;

/// <summary>
/// Класс для перевода данных различных бд в модели.
/// </summary>
public class TableToModelConverter
{
    /// <summary>
    /// Перевод строки из базы данных мпервого типа в модель.
    /// </summary>
    /// <param name="row">строка базы данных.</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    private static DtoJournalEntry ConvertFromMSSQL(DataRow row)
    {
        var dto = new DtoJournalEntry();

        var type = typeof(DtoJournalEntry);
        var properties = type.GetProperties()
               .Where(x => x.GetCustomAttribute<DatabaseNameAttribute>(true) is not null)
               .Where(x => x.GetCustomAttribute<DatabaseNameAttribute>(true)?.DBType == DatabaseTypes.MSSql)
               .Where(x => x.Name != nameof(DtoJournalEntry.EmployeeId))
               .Where(x => x.Name != nameof(DtoJournalEntry.NomenclatureId));

        // Перевод основных полей
        foreach (var property in properties)
        {
            var tableAttribute = property.GetCustomAttribute<DatabaseNameAttribute>() 
                ?? throw new NullReferenceException("attribute is null");

            if (row[tableAttribute.Name] is string value && value == "")
                continue;

            if (property.Name == nameof(DtoJournalEntry.TransactionDate) &&
                row[tableAttribute.Name] is DateTime dateTime)
                property.SetValue(dto,  new DateTimeOffset(dateTime));
            else
                property.SetValue(dto,  Convert.ChangeType(row[tableAttribute.Name], tableAttribute.DataType));
        }

        // Ветвление для поля id
        var transactionType = (TransactionType)dto.TransactionId;
        var isJobRelated = transactionType == TransactionType.JobStart || transactionType == TransactionType.JobFinish;
        
        var idProperty = isJobRelated ? 
            type.GetProperty(nameof(DtoJournalEntry.EmployeeId))
            : type.GetProperty(nameof(DtoJournalEntry.NomenclatureId));
        var tableIdAttribute = idProperty!.GetCustomAttribute<DatabaseNameAttribute>()
            ?? throw new NullReferenceException("attribute is null");
        if ((string)row[tableIdAttribute.Name] != "")
            idProperty!.SetValue(dto,  Convert.ChangeType(row[tableIdAttribute.Name], tableIdAttribute.DataType));
        
        return dto;
    }
    
    /// <summary>
    /// Перевод данных из таблицы в список моделей.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="dbType">Тип базы дынных</param>
    /// <returns></returns>
    public static IList<DtoJournalEntry> ConvertToDto(DataTable table, DatabaseTypes dbType)
    {
        var models = new List<DtoJournalEntry>();

        for (int position = 0; position < table.Rows.Count; position++)
        {
            var model = new DtoJournalEntry();

            switch (dbType)
            {
                case DatabaseTypes.MSSql:
                    model = ConvertFromMSSQL(table.Rows[position]);
                    break;
            }

           var result = PropertyValidator.ValidateModel(model);
           
           if (result)
                models.Add(model);
        }

        return models;
    }

}
