using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using University.Application.DTOs;
using University.Domain.Exceptions;

namespace University.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    // Micro-Optimization: Instantiate JsonSerializerOptions once to save memory allocation
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unhandled exception occurred while processing the request."
            );

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        // Prevent modifying the response if it has already started being sent to the client
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            DomainException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<string>
        {
            Success = false
        };

        if (exception is DomainException)
        {
            response.Message = exception.Message;
        }
        else
        {
            response.Message = "An internal server error occurred.";
        }

        // Using the static options field here for better performance
        var json = JsonSerializer.Serialize(response, _jsonOptions);

        await context.Response.WriteAsync(json);
    }
}