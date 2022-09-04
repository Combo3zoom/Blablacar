using System.Net;

namespace Blabalacar.Validations;

public class ExceptionHandler
{
    private readonly RequestDelegate _delegate;

    public ExceptionHandler(RequestDelegate @delegate)
    {
        _delegate = @delegate;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _delegate.Invoke(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var responce = string.Empty;
        var statusCode = HttpStatusCode.InternalServerError;
    }
}