using System.ComponentModel.DataAnnotations;

namespace Blabalacar.Models;

public class UserTrip
{

    public User User { get; set; }
    public int UserId { get; set; }
    public Trip Trip { get; set; }
    public int TripId { get; set; }
}