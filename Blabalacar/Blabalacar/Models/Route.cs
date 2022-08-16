using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class Route
{
    [Required]
    public string StartRoute { get; set; }
    [Required]
    public string EndRoute { get; set; }
}