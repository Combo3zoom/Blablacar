using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Blabalacar.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Role Role { get; set; }
    public bool IsVerification { get; set; }
    public DateTime CreateDateUser { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
}