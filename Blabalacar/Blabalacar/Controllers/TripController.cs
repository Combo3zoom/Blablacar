using System.Security.Claims;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Models.Entities.Trip;
using Blabalacar.Repository;
using Blabalacar.Service;
using Blabalacar.Service.TripService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Controllers;

[Route("[controller]")]
[Authorize]
public class TripController : Controller
{
    private readonly IRepository<Trip, Guid> _tripRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITripService _tripService;
    private readonly IUserRepository<UserTrip, Guid> _userRepository;
    private readonly IUserService _userService;

    public TripController(IRepository<Trip, Guid> tripRepository, IHttpContextAccessor httpContextAccessor,
        ITripService tripService, IUserRepository<UserTrip,Guid> userRepository, IUserService userService)
    {
        _tripRepository = tripRepository;
        _httpContextAccessor = httpContextAccessor;
        _tripService = tripService;
        _userRepository = userRepository;
        _userService = userService;
    }

    [HttpGet, AllowAnonymous]
    public  Task<IEnumerable<Trip>> GetTrips(CancellationToken cancellationToken=default)
        =>  _tripRepository.GetAll(cancellationToken);

    [HttpGet("{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> GetTripById(Guid id, CancellationToken cancellationToken=default)
    {
        var trip = await _tripRepository.GetById(id, cancellationToken);
        if (trip == null)
            return NotFound();
        
        return Ok(trip);
    } 

    [HttpPost]
    public async Task<IActionResult> CreateTrip(CreateTripBody createTripBody,
        CancellationToken cancellationToken=default)
    {
        var currentUserId = new Guid(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var currentUser = await _userRepository.GetById(currentUserId, cancellationToken);
        
        if (!ModelState.IsValid)
            return NotFound();

        var route = _tripService.CreateRoute(createTripBody);
        var trip = _tripService.CreateTrip(createTripBody, route);
        
        trip.Route.Trips!.Add(trip);
        
        var userTrip = _userService.FoundUserTrip(currentUser, trip);
        currentUser.UserTrips!.Add(userTrip);
        trip.UserTrips!.Add(userTrip);
        
        await _tripRepository.Insert(trip,cancellationToken);
        await _tripRepository.Save(cancellationToken);
        
        return CreatedAtAction(nameof(GetTrips), new {id = trip.Id}, trip);
    }

    [HttpPut]
    public async Task<IActionResult> PutTrip(UpdateTripBody newTrip, CancellationToken cancellationToken=default)
    {

        if (!ModelState.IsValid)
            return BadRequest();
        
        var currentTrip = await _tripRepository.GetById(newTrip.Id, cancellationToken);
        if (currentTrip == null)
            return NotFound();

        _tripService.UpdateTrip(currentTrip, newTrip);
        
        
        await _tripRepository.Save(cancellationToken);
        
        return Ok(currentTrip);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteTrip(Guid id, CancellationToken cancellationToken=default)
    {
        var deleteTrip = await _tripRepository.GetById(id,cancellationToken);
        if (deleteTrip == null)
            return BadRequest();
        
        await _tripRepository.Delete(deleteTrip, cancellationToken);
        await _tripRepository.Save(cancellationToken);;
        
        return Ok();
    }
}