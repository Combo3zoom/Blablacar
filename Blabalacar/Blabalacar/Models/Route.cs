using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Route
{
    public int Id { get; set; }
    public string StartRoute { get; set; }
    public string EndRoute { get; set; }
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}