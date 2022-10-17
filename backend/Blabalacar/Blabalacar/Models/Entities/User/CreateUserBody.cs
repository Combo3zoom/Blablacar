using System.ComponentModel.DataAnnotations;
using Blabalacar.Models;
using Blabalacar.Validations;

namespace Blabalacar.Controllers;

public class CreateUserBody
{
    [Required]
    [NameValidation("^[A-z]{1}[a-z]{3,16}$",ErrorMessage = "Name is incorrect")]
    public string Name { get; set; }
    public Role Role { get; set; }
    public bool IsVerification { get; set; }
}