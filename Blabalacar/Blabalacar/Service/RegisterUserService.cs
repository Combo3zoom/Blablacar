using System.Security.Claims;

namespace Blabalacar.Service;

public class RegisterUserService:IRegisterUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegisterUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetId()
    {
        var result = string.Empty;
        if (_httpContextAccessor != null)
            result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return result;
    }

    public string GetRefreshToken()
    {
        var currentUserId = GetId();
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}