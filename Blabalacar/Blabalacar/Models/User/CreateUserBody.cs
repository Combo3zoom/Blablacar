using Blabalacar.Models;

namespace Blabalacar.Controllers;

public class CreateUserBody
{
    public string Name { get; set; }
    public Role Role { get; set; }
    public bool IsVerification { get; set; }
}