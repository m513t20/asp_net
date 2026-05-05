using System;
using System.Data;
using Microsoft.Data.SqlClient;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Extensions;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Источник данных журнала. 
/// </summary>
public class JournalDataSource : IJournalDataSource
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
            end as productid,
            -- Код сотрудника
            case when transtype in (387, 386, 211, 216)
                then id
                else 0
            end as emploeeid,
            -- Код категории
            case when transtype = 101
                then categoryid
                else 0
            end as    categoryid ,
            -- Количество
            quantity,
            -- Цена
            price,
            -- Сумма скидки
            discountamount
        from journal
        where transtype in (387, 386, 211, 216, 101, 102)
        and transnumber > {1}
        order by transnumber desc";

    /// <summary>
    /// Подключение к клиентской базе данных.
    /// </summary>
    private readonly SqlConnection _connection;

    /// <summary>
    /// Настройки загрузки данных с клиента.
    /// </summary>
    private readonly LoadingSettingsModel _options;

    public JournalDataSource(SqlConnection connection, LoadingSettingsModel options)
    {
        _connection = connection;
        _options = options;
    }

    /// <summary>
    /// Извлекает набор новых (необработанных) строк журнала на основе настроек загрузки.
    /// </summary>
    /// <param name="branch"></param>
    /// <param name="batchSize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IEnumerable<JournalRowDto>> GetUnprocessedRowsAsync(BranchModel branch, int batchSize, CancellationToken token)
    {
        // Проверки
        ArgumentNullException.ThrowIfNull(_connection);
        var sql = string.Format(_sql, _options.BatchSize, _options.StartPosition);    

        try
        {
            if(_connection.State == System.Data.ConnectionState.Closed)
                await _connection.OpenAsync();
            // Выполняем выборку данных
            var command = new SqlCommand (sql, _connection);
            var dataset = new DataSet();
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataset);

            // Выполняем маппинг
            var result = from s in dataset.Tables[0].Rows.Cast<DataRow>()
                         select s.MapRow<JournalRowDto>();

            return result;
        }
        catch(Exception ex)
        {
            throw new InvalidDataException($"Невозможно выполнить SQL запрос {sql}\n{ex.Message}{ex.InnerException?.Message}");
        }
        finally
        {
            await _connection.CloseAsync();
        }   
    }
}
