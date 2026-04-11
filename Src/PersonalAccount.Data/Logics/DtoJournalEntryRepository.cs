using System;
using System.Data;
using System.Data.Common;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using PersonalAccount.Data.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using PersonalAccount.Domain.Models.Enums;
using PersonalAccount.Domain.Utils;

namespace PersonalAccount.Data.Logics;

/// <summary>
/// Репозиторий для сохранения данных журнала.
/// </summary>
public class DtoJournalEntryRepository
{
    // Шаблон SQL запроса
    private const string _sql = @"
       select top {0}
            -- Уникальный номер транзакции
            transnumber,
            -- Уникальный номер чека
            receiptn,
            -- Тип транзакции
            transtype,
            -- Период
            dater,
            -- Код номенклатуры
            case when transtype = 101
                then id 
                else 0 
            end as id,
            -- Код сотрудника
            case when transtype in (387, 386, 211, 216)
                then id
                else 0
            end as id,
            -- Код категории
            case when transtype = 101
                then categoryid
                else 0
            end as categoryid ,
            -- Количество
            quantity,
            -- Цена
            price,
            discountgroup,
            -- Сумма скидки
            discountamount,
            ManagerName,
            description
        from journal
        where transtype in (387, 386, 211, 216, 101, 102)
        and transnumber >= {1}";
    
    private PersonalAccountContext _context;

    private BulkConfig _bulkConfig;

    public DtoJournalEntryRepository(PersonalAccountContext context, LoadSettings settings, int timeout = 4)
    {
        _context = context;
        _bulkConfig = new BulkConfig
            {
                BatchSize = settings.PackSize,
                CalculateStats = true,
                BulkCopyTimeout = 4
            };
    }

    /// <summary>
    /// Получить выборку данных из журнала транзакций.
    /// </summary>
    /// <param name="connection"> Соединение. </param>
    /// <param name="options"> Опции. </param>
    /// <returns></returns>
    public async Task<IEnumerable<DtoJournalEntry>> GetRows(DbConnection connection, LoadSettings options)
    {
         // Проверки
        ArgumentNullException.ThrowIfNull(connection);
        var sql = string.Format(_sql, options.PackSize, options.StartPosition);    

        try
        {
            if(connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            // Выполняем выборку данных
            var command = new SqlCommand (sql, (SqlConnection) connection);
            var dataset = new DataSet();
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataset);

            // Выполняем маппинг
            var domainModels = TableToModelConverter.ConvertToDto(dataset.Tables[0], DatabaseTypes.MSSql);

            return domainModels;
        }
        catch(Exception ex)
        {
            throw new InvalidDataException($"Невозможно выполнить SQL запрос {sql}\n{ex.Message}{ex.InnerException?.Message}");
        }
        finally
        {
            await connection.CloseAsync();
        }   
    }


    public async Task SaveJournal(IEnumerable<DtoJournalEntry> models)
    {
        // Сохраняем
        var efJournal = models.Select( x => new Journal
            {
                Id = (int)x.Id,
                RecieptNumber = x.ReceiptNumber,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName,
                NomenclatureId = x.NomenclatureId,
                NomenclatureName = x.NomenclatureName,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                TransactionId = x.TransactionId,
                TransactionDate = x.TransactionDate.DateTime,
                Amount = x.Amount,
                Total = x.Total,
                Discount = x.Discount,
            }
        );

        await _context.BulkInsertAsync(efJournal, _bulkConfig);
    }
}
