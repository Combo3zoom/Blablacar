using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Trip
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public Route Route { get; set; }
    public DateTime DepartmentDate { get; set; }
    public DateTime CreateDateTrip { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
}