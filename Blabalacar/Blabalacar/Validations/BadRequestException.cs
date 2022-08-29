using System.Net;

namespace Blabalacar.Validations;

public class BadRequestException:BaseException
{
    public BadRequestException(string? message, int code, string description)
        : base(message, (int)HttpStatusCode.BadRequest, description)
    {
    }
}