using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;

[Route("[controller]")]
public class UserTripController: Controller
{
    private readonly BlalacarContext _context;
    public UserTripController(BlalacarContext context)
    {
        _context = context;
    }
    [HttpPost("{userId:int},{tripId:int}")]
    public IActionResult Post(int userId, int tripId)
    {
        if (!ModelState.IsValid)
                return NotFound();
        var user = _context.User.SingleOrDefault(stageUser => stageUser.Id == userId);
        var trip = _context.Trip.SingleOrDefault(stageTrip => stageTrip.Id == tripId);
        if (user == null || trip == null)
            return BadRequest();
        var userTrip = new UserTrip() {TripId = tripId, UserId = userId};
        user.UserTrips!.Add(userTrip);
        trip.UserTrips!.Add(userTrip);
        _context.SaveChanges();
        return Ok();
    }

    [HttpDelete("{userId:int}, {tripId:int}")]
    public IActionResult Delete(int userId, int tripId)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var userTrip = _context.UserTrips.SingleOrDefault(stageUserTrip =>
            stageUserTrip.UserId == userId && stageUserTrip.TripId == tripId);
        if (userTrip == null)
            return BadRequest();
        _context.User.Remove(_context.User.Find(userTrip.UserId)!); 
        _context.Trip.Remove(_context.Trip.Find(userTrip.TripId)!);
        _context.SaveChanges();
        return Ok();
    }
}