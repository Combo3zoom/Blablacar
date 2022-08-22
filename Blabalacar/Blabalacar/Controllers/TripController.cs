using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Controllers;
[Route("[controller]")]
public class TripController: Controller, IBaseCommand<Trip>
{
    private List<Trip> Trips = new List<Trip>(new[]
    {
        new Trip(){Route = new Route(){StartRoute = "Lviv", EndRoute = "Tryskavets"},
            DepartmentDate = DateTime.Parse("14:20:00")}
    });
    private int GetNextId() => Trips.Count == 0 ? 0 : Trips.Max(trip => trip.Id) + 1;
    private int GetNextRouteId() => Trips.Count == 0 ? 0 : Trips.Max(trip => trip.RouteId) + 1;

    [HttpGet]
    public IEnumerable<Trip> Get() => Trips;

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var trip = Trips.SingleOrDefault(trip => trip.Id == id);
        if (trip == null)
            return NotFound();
        return Ok(trip);
    }
    [HttpPost]
    public IActionResult Post([FromBody] Trip trip)
    {
        using (var context = new BlalacarContext())
        {
            if (!ModelState.IsValid)
                return NotFound();
            trip.Id = GetNextId();
            trip.Route.Id = GetNextRouteId();
            trip.Route.Trips.Add(trip);
            Trips.Add(trip);
            context.Trip.Add(trip);
            context.SaveChanges();
            return CreatedAtAction(nameof(Get),new{id=trip.Id}, trip);
        }
    }

    [HttpPut]
    public IActionResult Put(Trip trip)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var changedtrip = Trips.SingleOrDefault(storeTrip => storeTrip.Id == trip.Id);
        if (changedtrip == null)
            return NotFound();
        changedtrip.Route.StartRoute = trip.Route.StartRoute;
        changedtrip.Route.EndRoute = trip.Route.EndRoute;
        return Ok(changedtrip);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleteTrip = Trips.SingleOrDefault(storeTrip => storeTrip.Id == id);
        if (deleteTrip == null)
            return BadRequest();
        Trips.Remove(deleteTrip);
        return Ok();
    }
}