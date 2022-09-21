using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blabalacar.Models;

public class User: IdentityUser<Guid>
{
    //public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;
    public bool IsVerification { get; set; } = false;
    public DateTimeOffset UserCreatedAt { get; set; } = DateTime.Now;
    public ICollection<UserTrip>? UserTrips { get; set; } = new List<UserTrip>();
    
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset TokenCreated { get; set; }
    public DateTimeOffset TokenExpires { get; set; }
    public User(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public User(Guid id, string name, byte[] passwordHash, byte[] passwordSalt)
    {
        Id = id;
        Name = name;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public User()
    {
        
    }
}