using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestRegister.Models;
using TestRegister.Services;


namespace TestRegister.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    public static RegisterUser RegisterUser = new RegisterUser();
    private readonly IConfiguration _configuration;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IConfiguration configuration, IRegisterUserService registerUserService)
    {
        _configuration = configuration;
        _registerUserService = registerUserService;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        var userName = _registerUserService.GetName();
        return Ok(userName);

        // var userName = User.Identity!.Name;
        // var userName2 = User.FindFirstValue(ClaimTypes.Name);
        // var role = User.FindFirstValue(ClaimTypes.Role);
        // return Ok(userName);
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUser>> Register(RegisterUserDto request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        RegisterUser.UserName = request.Username;
        RegisterUser.PasswordHash = passwordHash;
        RegisterUser.PasswordSalt = passwordSalt;

        return Ok(RegisterUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(RegisterUserDto request)
    {
        if (RegisterUser.UserName != request.Username)
        {
            return BadRequest("User not found");
        }

        if (!VerifyPasswordHash(request.Password, RegisterUser.PasswordHash, RegisterUser.PasswordSalt))
        {
            return BadRequest("Wrong Password");
        }

        string token = CreateToken(RegisterUser);
        var refreshToken = GetRefreshToken();
        SetRefreshToken(refreshToken);
        return Ok(token);
    }
    

    private RefreshToken GetRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Created = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(5)
        };
        return refreshToken;
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!RegisterUser.RefreshToken.Equals(refreshToken))
            return Unauthorized("Invalid Refresh token");
        if (RegisterUser.TokenExpires < DateTime.Now)
            return Unauthorized("Token expired");
        string token = CreateToken(RegisterUser);
        var newRefreshToken = GetRefreshToken();
        SetRefreshToken(newRefreshToken);
        return Ok(token);
    }
    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        RegisterUser.RefreshToken = newRefreshToken.Token;
        RegisterUser.TokenCreated = newRefreshToken.Created;
        RegisterUser.TokenCreated = newRefreshToken.Expires;
    }

    private string CreateToken(RegisterUser registerUser)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, registerUser.UserName),
            new Claim(ClaimTypes.Role, "Admin")
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

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(RegisterUser.PasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash); //computedHash == passwordHash;
        }
    }
} 