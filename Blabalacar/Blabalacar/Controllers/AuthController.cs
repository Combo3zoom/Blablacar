using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Blabalacar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blabalacar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly BlalacarContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IConfiguration configuration, IRegisterUserService registerUserService,
        BlalacarContext context, UserManager<User> userManager, 
        SignInManager<User> signInManager)
    {
        _configuration = configuration;
        _registerUserService = registerUserService;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyId()
    {
        var userName = _registerUserService.GetId();
        return Ok(userName);
    }

    private Guid GetNextId() => new Guid();
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterUserDto request)
    {
        var newUser = new User{Id=GetNextId(), Name=request.Name, UserName = request.Name};
        var result = await _userManager.CreateAsync(newUser, request.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        await _signInManager.SignInAsync(newUser, isPersistent: false);//life time cookies - after close web
        
        return CreatedAtAction(nameof(GetMyId),new{id=newUser.Id}, newUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(RegisterUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _context.User.SingleOrDefaultAsync(currentUser => currentUser.Name == request.Name);// <-- extra error
        if (user==null)
            return BadRequest("User don't found ");

        var checkPassword = await _signInManager.PasswordSignInAsync(request.Name, request.Password, 
            false, false);
        if (!checkPassword.Succeeded)
            return BadRequest("Incorrect password");

        var result = await _signInManager.CanSignInAsync(user);

        var token = _registerUserService.CreateAccessToken(user, _configuration);
        _registerUserService.SetRefreshToken(user, Response);
        await _context.SaveChangesAsync();
        return Ok(token);
    }
    

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var currentIdUserCookies = _registerUserService.GetId();
        var user = _context.User.SingleOrDefault(currentUser => currentUser.Id.ToString() == currentIdUserCookies);//why Tostring?
        if (user == null)
            return BadRequest("user not found");
        
        var refreshToken = Request.Cookies["refreshToken"];
        if (!user.RefreshToken.Equals(refreshToken))
            return Unauthorized("Invalid Refresh token");
        
        if (user.TokenExpires < DateTimeOffset.Now)
            return Unauthorized("Token expired");
        
        _registerUserService.SetRefreshToken(user, Response);
        await _context.SaveChangesAsync();
        return Ok(_registerUserService.CreateAccessToken(user, _configuration));
    }

} 