using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Blabalacar.Service;

public class RegisterUserService:IRegisterUserService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegisterUserService(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
    {
        _memoryCache = memoryCache;
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetId()
    {
        var result = string.Empty;
        result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return result;
    }
    public void SetRefreshToken(Models.User? user)
    {
        user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshTokenCreatedAt = DateTime.Now;
        user.RefreshTokenExpiresAt = DateTime.Now.AddMinutes(300);
    }

    public string CreateAccessToken(Models.User? registerUser, IConfiguration _configuration)
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