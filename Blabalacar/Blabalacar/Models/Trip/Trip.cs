using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Trip
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    [Required]
    public Route Route { get; set; }
    [Required]
    public DateTime DepartureAt { get; set; }
    public DateTime TripCreatedAt { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();

    public Trip(int id, int routeId, Route route, DateTime departureAt)
    {
        Id = id;
        RouteId = routeId;
        Route = route;
        DepartureAt = departureAt;
    }

    private Trip()
    {
        
    }
}