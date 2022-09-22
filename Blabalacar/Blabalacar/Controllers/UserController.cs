using System.Linq;
using System.Security.Claims;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Repository;
using Blabalacar.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Controllers;
[Route("[controller]")]
[Authorize]
public class UserController: Controller
{
    private readonly IUserRepository<UserTrip, Guid> _userRepository;
    private readonly IRepository<Trip, Guid> _tripRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserRepository<UserTrip, Guid> userRepository,
        IRepository<Trip, Guid> tripRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _tripRepository = tripRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    private Guid GetNextId() => new Guid();

    [HttpGet, AllowAnonymous]
    public IEnumerable<User> Get() => _userRepository.GetAll().Result;
    
    [HttpGet("{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = _userRepository.GetById(id);
        return Ok(user);
    }
    [HttpPost("me/join to trip/{tripId:Guid}")]
    public async Task<IActionResult> JoinToTrip(Guid tripId)
    {
        var userId = new Guid(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!ModelState.IsValid)
            return NotFound();

        var user = _userRepository.GetById(userId).Result;
        var trip = _tripRepository.GetById(tripId).Result;
        
        if (user == null || trip == null)
            return BadRequest("user or trip don't exist");
        
        var userTrip = new UserTrip(user, userId, trip, tripId);
        
        user.UserTrips!.Add(userTrip);
        trip.UserTrips!.Add(userTrip);
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok();
    }

    [HttpPut, Authorize(Roles="Admin")]
    public async Task<IActionResult> Put(UpdateUserBody user)
    {
        if (!ModelState.IsValid)
            return BadRequest("Incorrect input dates");

        var changedUser = _userRepository.GetById(user.Id).Result;
        if (changedUser == null)
            return NotFound("user don't exist");
        
        changedUser.Name = user.Name;
        changedUser.Role = user.Role;
        changedUser.IsVerification = user.IsVerification;
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok(changedUser);
    }

    [HttpDelete("{userId:Guid}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UserDelete(Guid userId)
    {
        var deleteUser = _userRepository.GetById(userId).Result;
        if (deleteUser == null)
            return BadRequest("Incorrect Id");
        
        Response.Cookies.Delete("refreshToken");// <--- how to delete cookies in other user
        await _userRepository.Delete(deleteUser).ConfigureAwait(false);
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok();
    }
    [HttpDelete("me")]
    public async Task<IActionResult> SelfDelete()
    {
        var currentUserId = new Guid(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var deleteUser = _userRepository.GetById(currentUserId);
        if (deleteUser == null)
            return BadRequest("Incorrect Id");
        
        Response.Cookies.Delete("refreshToken");// <--- how to delete cookies in other user
        await _userRepository.Delete(await deleteUser.ConfigureAwait(false)).ConfigureAwait(false);
        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok();
    }
    

    [HttpDelete("me/trips/{tripId:Guid}")]
    public async Task<IActionResult> OrderTripDelete(Guid tripId)
    {
        var userId = new Guid(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!ModelState.IsValid)
            return NotFound("Use didn't found");

        var userTrip = _userRepository.GetFirstAndSecondModel(userId, tripId).Result;
        
        if (userTrip == null)
            return BadRequest("user wasn't attached to the trip");

        await _userRepository.ConnectionBetweenUserAndTripDelete(userTrip).ConfigureAwait(false);

        await _userRepository.Save().ConfigureAwait(false);
        
        return Ok();
    }
}