using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Trip:BaseEntity
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    [Required]
    public Route Route { get; set; }
    [Required]
    public DateTime DepartureAt { get; set; }
    public DateTime TripCreatedAt { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();

    public Trip(Guid id, Guid routeId, Route route, DateTime departureAt)
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