using System.Security.Claims;
using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Blabalacar.Models.Entities;
using Blabalacar.Repository;
using Blabalacar.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace Blabalacar.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository<UserTrip, Guid> _userRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IRegisterUserService _registerUserService;

    public AuthController(IConfiguration configuration, IRegisterUserService registerUserService, 
        UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository<UserTrip,Guid> userRepository,
        IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _registerUserService = registerUserService;
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _memoryCache = memoryCache;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyId()
    {
        _registerUserService.GetId();
        return Ok();
    }
    [HttpGet("logout/")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken=default)
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterUserDto request, CancellationToken cancellationToken=default)
    {
        var newUser = new User{Id=new Guid(), Name=request.Name, UserName = request.Name};
        var result = await _userManager.CreateAsync(newUser, request.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _signInManager.SignInAsync(newUser, isPersistent: false);

        return CreatedAtAction(nameof(GetMyId),new{id=newUser.Id}, newUser);
    } 

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(RegisterUserDto request, CancellationToken cancellationToken=default)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user =  await _userRepository.GetByName(request.Name, cancellationToken);
        
        if (user==null)
            return BadRequest("User don't found ");

        var checkPassword = await _signInManager.PasswordSignInAsync(request.Name, request.Password, 
            false, false);
        if (!checkPassword.Succeeded)
            return BadRequest("Incorrect name or password");

        await _signInManager.CanSignInAsync(user);

        var accessToken = _registerUserService.CreateAccessToken(user, _configuration);
        _registerUserService.SetRefreshToken(user);
        await _userRepository.Save(cancellationToken);
        

        SetCacheRefreshToken(user.Id, user.RefreshToken);

        var tokens = new TokenResponse(accessToken, user.RefreshToken);
        return Ok((tokens));
    }
    

    [HttpPost("refresh-token"),Authorize]
    public async Task<ActionResult<TokenResponse>> RefreshToken([FromQuery] string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        User? currentUser;
        if (!_memoryCache.TryGetValue(currentUserId, out currentUser))
        {
            currentUser = await _userRepository.GetById(currentUserId, cancellationToken);
            SetCacheRefreshToken(currentUserId, currentUser.RefreshToken);
        }


        if (currentUser.RefreshToken != refreshToken)
            return BadRequest("Incorrect refresh token");


        _registerUserService.SetRefreshToken(currentUser);
        await _userRepository.Save(cancellationToken);

        SetCacheRefreshToken(currentUserId, currentUser.RefreshToken);

        var accessToken = _registerUserService.CreateAccessToken(currentUser, _configuration);
        var tokens = new TokenResponse(accessToken, refreshToken);
        
        return Ok(tokens);
    }
    
    private void SetCacheRefreshToken(Guid currentUserId, string currentRefreshToken)
    {
        _memoryCache.Set(currentUserId, currentRefreshToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
        });
    }

} 