namespace Blabalacar.Models.Auto;

public class RefreshToken:BaseEntity
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; } = DateTime.Now;
    public DateTimeOffset Expires { get; set; }
}