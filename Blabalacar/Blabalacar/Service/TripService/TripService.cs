using Blabalacar.Controllers;
using Blabalacar.Models;
using Blabalacar.Models.Entities.Trip;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Service.TripService;

public class TripService:ITripService
{
    public Trip CreateTrip(CreateTripBody trip, Route route)
    {
        return new Trip{Id = new Guid(), RouteId = route.Id, Route = route,
            DepartureAt = trip.DepartureAt};;
    }

    public Route CreateRoute(CreateTripBody trip)
    {
        return new Route(new Guid(), trip.StartRoute, trip.EndRoute);
    }

    public void UpdateTrip(Trip currentTrip, UpdateTripBody newTrip)
    {
        currentTrip.Route.StartRoute = newTrip.Route.StartRoute;
        currentTrip.Route.EndRoute = newTrip.Route.EndRoute;
        currentTrip.DepartureAt = newTrip.DepartureAt;
    }
    
}