using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Repository;
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
    private Guid GetNextId() => new Guid();

    public TripController(IRepository<Trip, Guid> tripRepository, IHttpContextAccessor httpContextAccessor)
    {
        _tripRepository = tripRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<Trip>> Get() => await _tripRepository.GetAll().ConfigureAwait(false);

    [HttpGet("{id:Guid}"), AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var trip = _tripRepository.GetById(id).Result;
        if (trip == null)
            return NotFound();
        
        return Ok(trip);
    } 

    [HttpPost]
    public async Task<IActionResult> Post(CreateTripBody createTripBody)
    {
        if (!ModelState.IsValid)
            return NotFound();
        
        var nextIdTrip = GetNextId();
        var route = new Route(nextIdTrip, createTripBody.StartRoute, createTripBody.EndRoute);
        var trip = new Trip(nextIdTrip, route.Id, route, createTripBody.DepartureAt);
        trip.Route.Trips!.Add(trip);
        
        await _tripRepository.Insert(trip).ConfigureAwait(false);
        await _tripRepository.Save().ConfigureAwait(false);
        
        return CreatedAtAction(nameof(Get), new {id = trip.Id}, trip);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Trip trip)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var changedtrip = _tripRepository.GetById(trip.Id).Result;
        if (changedtrip == null)
            return NotFound();
        
        changedtrip.Route.StartRoute = trip.Route.StartRoute;
        changedtrip.Route.EndRoute = trip.Route.EndRoute;
        changedtrip.DepartureAt = trip.DepartureAt;
        changedtrip.UserTrips = trip.UserTrips;
        
        await _tripRepository.Save().ConfigureAwait(false);
        
        return Ok(changedtrip);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteTrip = _tripRepository.GetById(id).Result;
        if (deleteTrip == null)
            return BadRequest();
        
        await _tripRepository.Delete(deleteTrip).ConfigureAwait(false);
        await _tripRepository.Save().ConfigureAwait(false);;
        
        return Ok();
    }
}