using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Trip:IBaseEntity
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    [Required]
    public Route Route { get; set; }
    [Required]
    public DateTime DepartureAt { get; set; }
    public DateTime TripCreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
}