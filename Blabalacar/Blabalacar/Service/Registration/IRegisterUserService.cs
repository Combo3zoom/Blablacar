using Blabalacar.Models;

namespace Blabalacar.Service;

public interface IRegisterUserService
{
    public string GetId();
    public void SetRefreshToken(Models.User user);
    public string CreateAccessToken(Models.User registerUser, IConfiguration _configuration);
}