using Blabalacar.Controllers;
using Blabalacar.Models;
using Blabalacar.Models.Entities.Trip;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Service.TripService;

public interface ITripService
{
    Route CreateRoute(CreateTripBody trip);
    Trip CreateTrip(CreateTripBody trip, Route route);
    void UpdateTrip(Trip currentTrip, UpdateTripBody newTrip);
}