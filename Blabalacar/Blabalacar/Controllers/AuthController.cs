using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Blabalacar.Repository;
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
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository<UserTrip, Guid> _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IConfiguration configuration, IRegisterUserService registerUserService, 
        UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository<UserTrip,Guid> userRepository)
    {
        _configuration = configuration;
        _registerUserService = registerUserService;
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
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
        var result = await _userManager.CreateAsync(newUser, request.Password).ConfigureAwait(false);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        await _signInManager.SignInAsync(newUser, isPersistent: false).ConfigureAwait(false);//life time cookies - after close web
        
        return CreatedAtAction(nameof(GetMyId),new{id=newUser.Id}, newUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(RegisterUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = _userRepository.GetByName(request.Name).Result;
        //var user = await _context.User.SingleOrDefaultAsync(currentUser => currentUser.Name == request.Name);// <-- extra question
        if (user==null)
            return BadRequest("User don't found ");

        var checkPassword = await _signInManager.PasswordSignInAsync(request.Name, request.Password, 
            false, false).ConfigureAwait(false);
        if (!checkPassword.Succeeded)
            return BadRequest("Incorrect name or password");

        await _signInManager.CanSignInAsync(user).ConfigureAwait(false);//  <--?

        var token = _registerUserService.CreateAccessToken(user, _configuration);
        _registerUserService.SetRefreshToken(user);
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok(token);
    }
    

    [HttpPost("refresh-token"),Authorize]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var currentIdUserCookies = new Guid(_registerUserService.GetId());
        var user = _userRepository.GetById(currentIdUserCookies).Result;
        if (user == null)
            return BadRequest("user not found");
        
        var refreshToken = Request.Cookies["refreshToken"];
        if (!user.RefreshToken.Equals(refreshToken))
            return Unauthorized("Invalid Refresh token");
        
        if (user.TokenExpires < DateTimeOffset.Now)
            return Unauthorized("Token expired");
        
        _registerUserService.SetRefreshToken(user);
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok(_registerUserService.CreateAccessToken(user, _configuration));
    }

} 