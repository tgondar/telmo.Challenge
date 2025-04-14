using Application.Exceptions;
using Ardalis.GuardClauses;
using Backend.Challenge.Models.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Challenge.Middleware;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly IDictionary<Type, Func<HttpContext, Exception, CancellationToken, ValueTask<bool>>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, CancellationToken, ValueTask<bool>>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
            };
    }

    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.TryGetValue(exceptionType, out Func<HttpContext, Exception, CancellationToken, ValueTask<bool>>? func))
        {
            return func.Invoke(httpContext, exception, cancellationToken);
        }

        return HandleUnknownException(httpContext, exception, cancellationToken);
    }

    private async ValueTask<bool> HandleValidationException(
        HttpContext httpContext,
        Exception exception,
         CancellationToken cancellationToken)
    {
        var validationException = (ValidationException)exception;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        ErrorResponse errorResponse = new ErrorResponse
        {
            Errors = validationException.Errors
                .SelectMany(error => error.Value
                    .Select(errorDetail => new ErrorModel(errorDetail.ErrorCode, error.Key, errorDetail.Message!)))
                .ToList()
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private async ValueTask<bool> HandleNotFoundException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ErrorResponse errorResponse = new ErrorResponse(StatusCodes.Status404NotFound.ToString(), exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private static async ValueTask<bool> HandleUnknownException(
    HttpContext context,
    Exception exception,
    CancellationToken cancellationToken)
    {
        var errorResponse = new ErrorResponse("", exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}

