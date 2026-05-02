using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Extensions;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Console.Logics;

/// <summary>
/// Реализация интерфейса <see cref="IClientRepository<T>"/>
/// </summary>
public class JournalReadRepository : IClientRepository<JournalRowDto>
{
    // Шаблон SQL запроса
    private const string _sql = @"
    select
        tt.*,
        t2.[description] as category_name,
        t3.[description] as product_name,
        t4.fullname as employee_name
    from 
    (    
        select top  {0}
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
                then t1.id 
                else 0 
            end as productid,
            -- Код сотрудника
            case when transtype in (387, 386)
                then t1.id
                else 0
            end as emploeeid,
            -- Код категории
            case when transtype = 101
                then t1.categoryid
                else 0
            end as    categoryid ,
            -- Количество
            quantity,
            -- Цена
            price,
            -- Сумма скидки
            discountamount
        from journal as t1
        where transtype in (387, 386, 211, 216, 101, 102)
        and transnumber > {1}
    ) as tt 
    left join category as t2 on t2.id = tt.categoryid
    left join product as t3 on t3.productid = tt.productid
    left join vw_Personnel as t4 on t4.personid = tt.emploeeid
    order by transnumber ";


    /// <summary>
    /// Получить выборку данных из журнала транзакций.
    /// </summary>
    /// <param name="connection"> Соединение. </param>
    /// <param name="options"> Опции. </param>
    /// <returns></returns>
    public async Task<IEnumerable<JournalRowDto>> GetRows(DbConnection connection, LoadingSettingsModel options)
    {
        // Проверки
        ArgumentNullException.ThrowIfNull(connection);
        var sql = string.Format(_sql, options.BatchSize, options.StartPosition);

        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            // Выполняем выборку данных
            var command = new SqlCommand(sql, (SqlConnection)connection);
            var dataset = new DataSet();
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataset);

            // Выполняем маппинг
            var result = from s in dataset.Tables[0].Rows.Cast<DataRow>()
                         select s.MapRow<JournalRowDto>();

            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"Невозможно выполнить SQL запрос {sql}\n{ex.Message}{ex.InnerException?.Message}");
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
