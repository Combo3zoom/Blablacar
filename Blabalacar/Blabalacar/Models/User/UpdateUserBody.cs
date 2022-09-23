using System.ComponentModel.DataAnnotations;
using Blabalacar.Models;
using Blabalacar.Validations;



public class UpdateUserBody
{
    public Guid Id { get; set; }
    [Required]
    [NameValidation("^[A-z]{1}[a-z]{3,16}$",ErrorMessage = "Name is incorrect")]
    public string Name { get; set; }
    public bool IsVerification { get; set; }
    public Role Role { get; set; }
}