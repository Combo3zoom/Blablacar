using System.Linq;
using System.Security.Claims;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Models.Entities.User;
using Blabalacar.Repository;
using Blabalacar.Service;
using Blabalacar.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blabalacar.Controllers;
[Route("[controller]")]
[Authorize]
public class UserController: Controller
{

    private readonly IUserRepository<UserTrip, Guid> _userRepository;
    private readonly IRepository<Trip, Guid> _tripRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IMemoryCache _memoryCache;

    public UserController(IUserRepository<UserTrip, Guid> userRepository,
        IRepository<Trip, Guid> tripRepository, IHttpContextAccessor httpContextAccessor, IUserService userService,
        IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _tripRepository = tripRepository;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _memoryCache = memoryCache;
    }

    [HttpGet, AllowAnonymous]
    public Task<IEnumerable<User?>> Get(CancellationToken cancellationToken=default)
        => _userRepository.GetAll(cancellationToken);
    
    [HttpGet("/me"), Authorize]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken=default)
    {
        var id = new Guid(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var user = await _userRepository.GetById(id, cancellationToken);
        return Ok(user);
    }
    
    [HttpGet("{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken=default)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        return Ok(user);
    }
    
    [HttpGet("/me/trips"), Authorize]
    public async Task<IActionResult> GetMyTrips(CancellationToken cancellationToken=default)
    {
        var id = new Guid(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
        var user = await _userRepository.GetById(id, cancellationToken);
        var userTrips = user.UserTrips;
        List<Trip> trips = new List<Trip>();

        foreach (var userTrip in userTrips)
        {
            trips.Add((await _tripRepository.GetById(userTrip.TripId, cancellationToken))!);
        }
        return Ok(trips);
    }
    
    [HttpPost("me/trips/{tripId}/joinToTrip")]
    public async Task<IActionResult> JoinToTrip([FromRoute]Guid tripId, CancellationToken cancellationToken=default)
    {
        var userId = new Guid(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!ModelState.IsValid)
            return NotFound();

        var user = await _userRepository.GetById(userId, cancellationToken);
        var trip = await _tripRepository.GetById(tripId, cancellationToken);

        if (user == null || trip == null)
            return BadRequest("user or trip don't exist");

        var userTrip = _userService.CreateUserTrip(user, trip);
        user.UserTrips!.Add(userTrip);
        trip.UserTrips!.Add(userTrip);

        await _userRepository.Save(cancellationToken);
        
        _memoryCache.Remove(userId);
        
        return Ok();
    }

    [HttpPut("admin/put user"), Authorize(Roles="Admin")]
    public async Task<IActionResult> AdminPutUser(AdminUpdateUserBody newUser, CancellationToken cancellationToken=default)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect input dates");

        var currentUser = await _userRepository.GetById(newUser.Id, cancellationToken);
        if (currentUser == null)
            return NotFound("user don't exist");
        
        _userService.AdminUpdateUser(currentUser, newUser);
        await _userRepository.Save(cancellationToken);
        
        _memoryCache.Remove(currentUser.Id);
        
        return Ok(currentUser);
    }
    
    [HttpPut("me/put"), Authorize]
    public async Task<IActionResult> PutUser([FromBody]UpdateUserBody newUser, CancellationToken cancellationToken=default)
    {
        var userId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!ModelState.IsValid)
            return BadRequest("Incorrect input dates");

        var currentUser = await _userRepository.GetById(userId, cancellationToken);
        if (currentUser == null)
            return NotFound("user don't exist");
        
        _userService.UpdateSelfUser(currentUser, newUser);
        await _userRepository.Save(cancellationToken);
        
        _memoryCache.Remove(currentUser.Id);

        return Ok(currentUser);
    }

    [HttpDelete("Admin/{userId:Guid}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminDeleteUser(Guid userId, CancellationToken cancellationToken=default)
    {
        var deleteUser = await _userRepository.GetById(userId,cancellationToken);
        if (deleteUser == null)
            return BadRequest("Incorrect Id");
        
        await _userRepository.Delete(deleteUser, cancellationToken);
        await _userRepository.Save(cancellationToken);
        
        _memoryCache.Remove(deleteUser.Id);
        
        return Ok();
    }

    [HttpDelete("me/trips/{tripId:Guid}")]
    public async Task<IActionResult> OrderTripDelete(Guid tripId, CancellationToken cancellationToken=default)
    {
        var userId = new Guid(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!ModelState.IsValid)
            return NotFound("Use didn't found");

        var userTrip = await _userRepository.GetUserTrips(userId, tripId, cancellationToken);
        
        if (userTrip == null)
            return BadRequest("user wasn't attached to the trip");

        await _userRepository.ConnectionBetweenUserAndTripDelete(userTrip, cancellationToken);

        await _userRepository.Save(cancellationToken);
        
        _memoryCache.Remove(userId);
        
        return Ok();
    }
}