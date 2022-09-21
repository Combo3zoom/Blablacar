using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Microsoft.IdentityModel.Tokens;

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
    public void SetRefreshToken(User user, HttpResponse response)
    {
        var newRefreshToken = new RefreshToken // create refresh token
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Created = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(300)
        };

        var cookieOptions = new CookieOptions //option cookies
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        
        response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions); // add cookies 
        
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }

    public string CreateAccessToken(User registerUser, IConfiguration _configuration)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, registerUser.Name),
            new Claim(ClaimTypes.Role, registerUser.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, registerUser.Id.ToString())
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwt;
    }
}