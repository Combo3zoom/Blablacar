namespace Blabalacar.Validations;

public class BaseException:Exception
{
    public int Code { get; }
    public string Description { get; }

    public BaseException(string? message, int code, string description) : base(message)
    {
        Code = code;
        Description = description;
    }
}