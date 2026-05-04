using System;
using System.Data.Common;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;
using Npgsql;
using NpgsqlTypes;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса IServerRepository
/// </summary>
public class JournalWriteRepository : IServerRepository<JournalRowDto>
{
    // Шаблон SQL запроса
    private const string _sql = @"
        COPY journal (
            transnumber,
            transtype,
            receiptn,
            productid,
            product_name,
            categoryid,
            category_name,
            emploeeid,
            emploee_name,
            dater,
            quantity,
            price,
            discountamount,
            company_id,
            branch_id 
        ) 
        FROM STDIN (FORMAT BINARY)";

    /// <inheritdoc/>
    public async Task<LoadingSettingsModel?> SaveRows(DbConnection connection, IEnumerable<JournalRowDto> transactions, LoadingSettingsModel options)
    {
        ArgumentNullException.ThrowIfNull(connection);
        if (options.Branch?.Id == Guid.Empty)
            throw new ArgumentException("Некорректно переданы настройки! Уникальный код филиала не указан!");

        if (options.Branch?.Owner.Id == Guid.Empty)
            throw new ArgumentException("Некорректно переданы настройки! Уникальный код организации не указан!");
            
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            // Открываем потоковую запись данных
            using var writer = (connection as NpgsqlConnection)!.BeginBinaryImport(_sql);
            foreach (var transaction in transactions)
            {
                writer.StartRow();

                // transnumber
                writer.Write(transaction.Code, NpgsqlDbType.Bigint);
                // transtype
                writer.Write(transaction.TypeCode, NpgsqlDbType.Bigint);
                // receiptn
                writer.Write(transaction.ReceiptNumber, NpgsqlDbType.Bigint);

                // productid
                if (transaction.ProductCode.HasValue)
                    writer.Write(transaction.ProductCode, NpgsqlDbType.Bigint);
                else
                    writer.WriteNull();

                // product_name
                if (!string.IsNullOrEmpty(transaction.ProductName))
                    writer.Write(transaction.ProductName, NpgsqlDbType.Text);
                else
                    writer.WriteNull();

                // categoryid
                if (transaction.CategoryCode.HasValue)
                    writer.Write(transaction.CategoryCode, NpgsqlDbType.Bigint);
                else
                    writer.WriteNull();

                // category_name
                if (!string.IsNullOrEmpty(transaction.CategoryName))
                    writer.Write(transaction.CategoryName, NpgsqlDbType.Text);
                else
                    writer.WriteNull();

                // emploeeid
                if (transaction.EmploeeCode.HasValue)
                    writer.Write(transaction.EmploeeCode, NpgsqlDbType.Bigint);
                else
                    writer.WriteNull();

                // emploee_name
                if (!string.IsNullOrEmpty(transaction.EmploeeName))
                    writer.Write(transaction.EmploeeName, NpgsqlDbType.Text);
                else
                    writer.WriteNull();

                // dater
                writer.Write(DateTime.SpecifyKind(transaction.Period, DateTimeKind.Utc), NpgsqlDbType.TimestampTz);
                // quantity
                writer.Write(transaction.Quantity, NpgsqlDbType.Double);
                // price
                writer.Write(transaction.Price, NpgsqlDbType.Double);
                // discountamount
                writer.Write(transaction.Discount, NpgsqlDbType.Double);
                // company_id
                writer.Write(options.Branch!.Owner.Id, NpgsqlDbType.Uuid);
                // branch_id
                writer.Write(options.Branch!.Id, NpgsqlDbType.Uuid);
            }

            writer.Complete();

            // Формируем ответный вариант настроек
            options.StartPosition = transactions.Max(x => x.Code);
            return options;
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"Невозможно выполнить вставку данных!\n{ex.Message}{ex.InnerException?.Message}");
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
