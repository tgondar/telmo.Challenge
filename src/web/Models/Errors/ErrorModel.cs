namespace Backend.Challenge.Models.Errors;

public class ErrorModel
{
    public ErrorModel()
    {
    }

    public ErrorModel(string message)
    {
        Message = message;
    }

    public ErrorModel(
        string code,
        string message) : this(message)
    {
        Code = code;
        Message = message;
    }

    public ErrorModel(
        string code,
        string property,
        string message) : this(code, message)
    {
        Code = code;
        Property = property;
        Message = message;
    }

    public string Code { get; set; }
    public string Property { get; set; }
    public string Message { get; set; }
}
