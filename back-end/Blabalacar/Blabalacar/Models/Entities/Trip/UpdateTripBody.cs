using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models.Entities.Trip;

public class UpdateTripBody
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Route Route { get; set; }
    [Required]
    public DateTime DepartureAt { get; set; }
}