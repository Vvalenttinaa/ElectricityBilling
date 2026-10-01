using ElectricityBilling.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace ElectricityBilling.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await WriteResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}