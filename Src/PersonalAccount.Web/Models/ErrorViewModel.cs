using System;
using PersonalAccount.Domain.Core;

namespace PersonalAccount.Web.Models;

public class ErrorViewModel : IErrorText
{
    /// <summary>
    /// Заголовок ошибки
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string ErrorText { get; set; } = null!;

    /// <summary>
    /// Трейс ошибки
    /// </summary>
    public string StackTrace { get; set; } = null!;

    /// <summary>
    /// Флаг наличие ошибки
    /// </summary>
    public bool IsError => !string.IsNullOrWhiteSpace(ErrorText);
}
