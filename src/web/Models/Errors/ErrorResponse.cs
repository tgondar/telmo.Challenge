using System.Collections.Generic;

namespace Backend.Challenge.Models.Errors;

public class ErrorResponse
{
    public ErrorResponse()
    {
        Errors = [];
    }

    public ErrorResponse(
        string code,
        string message)
    {
        Errors = [new ErrorModel(code, message)];
    }

    public ErrorResponse(List<ErrorModel> errors)
    {
        Errors = errors;
    }

    public ErrorResponse(ErrorModel error)
    {
        Errors = [error];
    }

    public List<ErrorModel> Errors { get; set; }
}
