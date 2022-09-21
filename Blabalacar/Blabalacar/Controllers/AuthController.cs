using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Blabalacar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blabalacar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    public static User RegisterUser;
    private readonly BlalacarContext _context;
    private readonly IConfiguration _configuration;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IConfiguration configuration, IRegisterUserService registerUserService,
        BlalacarContext context)
    {
        _configuration = configuration;
        _registerUserService = registerUserService;
        _context = context;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        var userName = _registerUserService.GetName();
        return Ok(userName);
    }

    private int GetNextId() => _context.User.Local.Count == 0 ? 0 : _context.User.Local.Max(user => user.Id) + 1;
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterUserDto request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var newUser = new User(GetNextId(), request.Name, passwordHash, passwordSalt);
        RegisterUser = newUser;
        _context.User.Add(newUser);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMe),new{id=newUser.Id}, newUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(RegisterUserDto request)
    {
        RegisterUser = _context.User.SingleOrDefault(user=> user.Name==request.Name)!;
        if (RegisterUser==null)
            return BadRequest("User not found ");
        if (!VerifyPasswordHash(request.Password, RegisterUser.PasswordHash, RegisterUser.PasswordSalt))
            return BadRequest("incorrect password");

        var token = CreateToken(RegisterUser);
        var refreshToken = GetRefreshToken();
        SetRefreshToken(refreshToken);
        await _context.SaveChangesAsync();
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
        await _context.SaveChangesAsync();
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

    private string CreateToken(User registerUser)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, registerUser.Name),
            new Claim(ClaimTypes.Role, Role.User.ToString()),//?
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

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash); //computedHash == passwordHash;
    }
} 