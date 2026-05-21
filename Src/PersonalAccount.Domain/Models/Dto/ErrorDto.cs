using System;
using PersonalAccount.Domain.Core;

namespace PersonalAccount.Domain.Models.Dto;

/// <summary>
/// Описание ошибки
/// </summary>
public class ErrorDto : IErrorText
{
    // флаг
    public bool IsError => !string.IsNullOrWhiteSpace(ErrorText);

    /// <summary>
    /// Текст ощиюкт
    /// </summary>
    public string ErrorText { get; set; } = null!;

    public string StackTrace { get; set; } = null!;
}
