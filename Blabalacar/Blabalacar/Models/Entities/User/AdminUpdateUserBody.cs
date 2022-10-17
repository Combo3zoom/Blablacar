using System.ComponentModel.DataAnnotations;
using Blabalacar.Validations;

namespace Blabalacar.Models.Entities.User;

public class AdminUpdateUserBody
{
    public Guid Id { get; set; }
    public bool IsVerification { get; set; }

}