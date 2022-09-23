using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Route:BaseEntity
{
    public Guid Id { get; set; }
    [Required]
    public string StartRoute { get; set; }
    [Required]
    public string EndRoute { get; set; }
    public ICollection<Trip>? Trips { get; set; } = new List<Trip>();

    public Route(Guid id, string startRoute, string endRoute)
    {
        Id = id;
        StartRoute = startRoute;
        EndRoute = endRoute;
    }

    private Route()
    {
        
    }
}