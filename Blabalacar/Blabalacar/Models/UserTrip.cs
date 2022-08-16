namespace Blabalacar.Models;

public class UserTrip
{
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Trip Trip { get; set; }
    public int TripId { get; set; }
}