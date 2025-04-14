using FluentValidation.Results;

namespace Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, ValidationExceptionModel[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
                   .GroupBy(e => e.PropertyName)
                   .ToDictionary(
                       g => g.Key,
                       g => g.Select(failure => new ValidationExceptionModel
                       {
                           ErrorCode = failure.ErrorCode,
                           Message = failure.ErrorMessage
                       }).ToArray());

    }

    public IDictionary<string, ValidationExceptionModel[]> Errors { get; }
}

public class ValidationExceptionModel
{
    public string? ErrorCode { get; set; }

    public string Message { get; set; } = string.Empty;
}
