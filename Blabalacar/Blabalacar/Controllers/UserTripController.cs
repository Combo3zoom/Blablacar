using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;

[Route("[controller]")]
public class UserTripController: Controller
{

    [HttpPost]
    public IActionResult Post(int userId, int tripId)
    {
        using (var context = new BlalacarContext())
        {
            if (!ModelState.IsValid)
                return NotFound();
            var user = context.User.SingleOrDefault(stageUser => stageUser.Id == userId);
            var trip = context.Trip.SingleOrDefault(stageTrip => stageTrip.Id == tripId);
            if (user == null || trip == null)
                return BadRequest();
            var a = new UserTrip() {TripId = tripId, UserId = userId};
            user.UserTrips!.Add(a);
            trip.UserTrips!.Add(a);
            context.SaveChanges();
            return Ok();
        }
    }

    [HttpDelete("{userId:int}, {tripId:int}")]
    public IActionResult Delete(int userId, int tripId)
    {
        using (var context = new BlalacarContext())
        {
            if (!ModelState.IsValid)
                return NotFound();
            var userTrip = context.UserTrips.SingleOrDefault(stageUserTrip =>
                stageUserTrip.UserId == userId && stageUserTrip.TripId == tripId);
            if (userTrip == null)
                return BadRequest();
            context.User.Remove(context.User.Find(userTrip.UserId)!); 
            context.Trip.Remove(context.Trip.Find(userTrip.TripId)!);
            context.SaveChanges();
            return Ok();
        }
    }
}