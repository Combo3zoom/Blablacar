using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class UserTrip
{

    public User User { get; set; }
    public Guid UserId { get; set; }
    public Trip Trip { get; set; }
    public Guid TripId { get; set; }

    
}