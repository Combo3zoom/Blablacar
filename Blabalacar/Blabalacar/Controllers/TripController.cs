using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Controllers;

[Route("[controller]")]
public class TripController : Controller
{
    private readonly BlalacarContext _context;
    private int GetNextId() => _context.Trip.Local.Count == 0 ? 0 : _context.Trip.Local.Max(trip => trip.Id) + 1;

    private int GetNextRouteId() =>
        _context.Trip.Local.Count == 0 ? 0 : _context.Trip.Local.Max(trip => trip.Route.Id) + 1;

    public TripController(BlalacarContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Trip>> Get() => _context.Trip.Include("UserTrips").Include("Route");

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var trip = _context.Trip.Include("UserTrips").Include("Route").
            SingleOrDefault(trip => trip.Id == id);
        if (trip == null)
            return NotFound();
        return Ok(trip);
    } 

    [HttpPost]
    public IActionResult Post(CreateTripBody createTripBody)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var nextIdTrip = GetNextId();
        var route = new Route(GetNextRouteId(), createTripBody.StartRoute, createTripBody.EndRoute);
        var trip = new Trip(nextIdTrip, route.Id, route, createTripBody.DepartureAt);
        trip.Route.Trips!.Add(trip);
        _context.Trip.Add(trip);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new {id = trip.Id}, trip);
    }

    [HttpPut]
    public IActionResult Put(Trip trip)
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
        _context.SaveChanges();
        return Ok(changedtrip);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleteTrip = _context.Trip.SingleOrDefault(storeTrip => storeTrip.Id == id);
        if (deleteTrip == null)
            return BadRequest();
        _context.Trip.Remove(deleteTrip);
        _context.SaveChanges();
        return Ok();
    }
}