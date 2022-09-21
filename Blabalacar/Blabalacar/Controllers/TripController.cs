using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Controllers;

[Route("[controller]")]
[Authorize]
public class TripController : Controller
{
    private readonly BlalacarContext _context;
    private Guid GetNextId() => new Guid();

    public TripController(BlalacarContext context)
    {
        _context = context;
    }

    [HttpGet, AllowAnonymous]
    public async Task<IEnumerable<Trip>> Get() => _context.Trip.Include("UserTrips").Include("Route");

    [HttpGet("{id:Guid}"), AllowAnonymous]
    public Task<IActionResult> Get(Guid id)
    {
        var trip =  _context.Trip.Include("UserTrips").Include("Route").
            SingleOrDefault(trip => trip.Id == id);
        if (trip == null)
            return Task.FromResult<IActionResult>(NotFound());
        return Task.FromResult<IActionResult>(Ok(trip));
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
        _context.Trip.Add(trip);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new {id = trip.Id}, trip);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Trip trip)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var changedtrip = _context.Trip.SingleOrDefault(storeTrip => storeTrip.Id == trip.Id);
        if (changedtrip == null)
            return NotFound();
        changedtrip.Route.StartRoute = trip.Route.StartRoute;
        changedtrip.Route.EndRoute = trip.Route.EndRoute;
        changedtrip.DepartureAt = trip.DepartureAt;
        changedtrip.UserTrips = trip.UserTrips;
        await _context.SaveChangesAsync();
        return Ok(changedtrip);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteTrip = _context.Trip.SingleOrDefault(storeTrip => storeTrip.Id == id);
        if (deleteTrip == null)
            return BadRequest();
        _context.Trip.Remove(deleteTrip);
        await _context.SaveChangesAsync();
        return Ok();
    }
}