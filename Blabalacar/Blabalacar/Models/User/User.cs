using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Blabalacar.Validations;

namespace Blabalacar.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    public Role Role { get; set; }
    public bool IsVerification { get; set; }
    public DateTime UserCreatedAt { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
    public User(int id, string name, Role role, bool isVerification)
    {
        Id = id;
        Name = name;
        Role = role;
        IsVerification = isVerification;
    }

    private User()
    {
        
    }
}