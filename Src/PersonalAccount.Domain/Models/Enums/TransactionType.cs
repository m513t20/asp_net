namespace PersonalAccount.Domain.Models.Enums;

/// <summary>
/// Перечисление типов транзакций
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// PLU Sales.
    /// </summary>
    PLUSale = 101,

    /// <summary>
    /// Списание по себестоимости.
    /// </summary>
    WriteOff = 111,

    
    /// <summary>
    /// Оплата наличными.
    /// </summary>
    Cash = 211,

    /// <summary>
    /// Оплата картой.
    /// </summary>
    Visa = 216,

    /// <summary>
    /// Вход в систему.
    /// </summary>
    Login = 301,

    /// <summary>
    /// Начало работы
    /// </summary>
    JobStart = 386,

    /// <summary>
    /// Окончание работы.
    /// </summary>
    JobFinish = 387,

    /// <summary>
    /// Итоговое значение.
    /// </summary>
    Total = 501,
}
