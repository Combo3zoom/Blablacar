using System.ComponentModel.DataAnnotations;
using Blabalacar.Validations;

namespace Blabalacar.Models.Entities.User;

public class UpdateUserBody
{
    [Required]
    [NameValidation("^[A-z]{1}[a-z]{3,16}$",ErrorMessage = "Name is incorrect")]
    public string? Name { get; set; }
}