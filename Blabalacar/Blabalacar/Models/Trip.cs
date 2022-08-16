using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Trip
{
    public int Id { get; set; }
    [Required]
    public Route Route { get; set; }
    public DateTime DepartmentDate { get; set; }
    public ICollection<UserTrip> UserTrips { get; set; }
}