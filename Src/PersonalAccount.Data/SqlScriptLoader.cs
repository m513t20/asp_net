using System;
using System.Reflection;

namespace PersonalAccount.Data;

/// <summary>
/// Класс для чтения sql скриптов.
/// </summary>
public class SqlScriptLoader
{
    /// <summary>
    /// Прочитать файл из папки Sql
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private static string ReadFile(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream($"PersonalAccount.Data.Sql.{filename}")
            ?? throw new ArgumentNullException($"couldn't find fitting assembly for {filename}");
        using StreamReader reader = new StreamReader(stream);
        string content = reader.ReadToEnd();
        return content;
    }

    /// <summary>
    /// Скрипт создания таблиц.
    /// </summary>
    public static string CreateScript { get => ReadFile("CreateTables.sql"); }
    
    /// <summary>
    /// Скрипт удаления таблиц.
    /// </summary>
    public static string DropScript { get => ReadFile("DropTables.sql"); }
    
    /// <summary>
    /// Скрипт заполнения таблиц.
    /// </summary>
    public static string FillScript { get => ReadFile("FillTables.sql"); }
}
