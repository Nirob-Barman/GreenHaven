using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = CreateProblemDetails(context, exception);

        if (problemDetails.Status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }
        else
        {
            _logger.LogWarning(exception, "Request failed with {StatusCode}", problemDetails.Status);
        }

        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => CreateValidationProblemDetails(context, validationException),
            NotFoundException => CreateProblemDetails(context, StatusCodes.Status404NotFound, "Not found", exception.Message),
            ForbiddenAccessException => CreateProblemDetails(context, StatusCodes.Status403Forbidden, "Forbidden", exception.Message),
            UnauthorizedAccessException => CreateProblemDetails(context, StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            DomainException => CreateProblemDetails(context, StatusCodes.Status400BadRequest, "Bad request", exception.Message),
            _ => CreateProblemDetails(context, StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.")
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        int status,
        string title,
        string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };
    }
}
