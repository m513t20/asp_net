using System;
using Microsoft.AspNetCore.Http;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Миддлвар для отлова ошибок
/// </summary>
/// <param name="next"></param>
public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Обязательный метод для обработки
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleException(context, ex);    
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        // Модель с данными
        var model = new ErrorDto()
        {
          ErrorText = $"{exception.Message}{exception.InnerException?.Message}",
          StackTrace = exception.StackTrace ?? string.Empty  
        };

        // готовим контекст
        context.Response.Clear();
        context.Response.StatusCode = exception.GetType() == typeof(KeyNotFoundException) ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        
        // загружаем модель в контекст
        await context.Response.WriteAsJsonAsync(model);
    }
}
