using Blabalacar.Models;

namespace Blabalacar.Service;

public interface IRegisterUserService
{
    public string GetId();
    public void SetRefreshToken(User user);
    public string CreateAccessToken(User registerUser, IConfiguration _configuration);
}