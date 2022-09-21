using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;

[Route("[controller]")]
[Authorize]
public class UserTripController: Controller
{
    private readonly BlalacarContext _context;
    public UserTripController(BlalacarContext context)
    {
        _context = context;
    }
    [HttpPost("{userId:int},{tripId:int}")]
    public async Task<IActionResult> Post(int userId, int tripId)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var user = _context.User.SingleOrDefault(stageUser => stageUser.Id == userId);
        var trip = _context.Trip.SingleOrDefault(stageTrip => stageTrip.Id == tripId);
        if (user == null || trip == null)
            return BadRequest();
        var userTrip = new UserTrip(user, userId, trip, tripId);
        user.UserTrips!.Add(userTrip);
        trip.UserTrips!.Add(userTrip);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{userId:int}, {tripId:int}")]
    public async Task<IActionResult> Delete(int userId, int tripId)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var userTrip = _context.UserTrips.SingleOrDefault(stageUserTrip =>
            stageUserTrip.UserId == userId && stageUserTrip.TripId == tripId);
        if (userTrip == null)
            return BadRequest();
        _context.User.Remove((await _context.User.FindAsync(userTrip.UserId))!); 
        _context.Trip.Remove((await _context.Trip.FindAsync(userTrip.TripId))!);
        await _context.SaveChangesAsync();
        return Ok();
    }
}