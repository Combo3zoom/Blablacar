using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blabalacar.Models;

public class User: IdentityUser<Guid>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;
    public bool IsVerification { get; set; } = false;
    public DateTimeOffset UserCreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
    
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenCreatedAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}