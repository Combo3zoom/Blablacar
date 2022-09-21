using System.Security.Claims;

namespace Blabalacar.Service;

public class RegisterUserService:IRegisterUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegisterUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetName()
    {
        var result = string.Empty;
        if (_httpContextAccessor != null)
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return result;
    }
}