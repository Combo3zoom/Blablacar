using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;
[Route("[controller]")]
public class UserController: Controller, IBaseCommand<User>
{
    public List<User> Users = new List<User>(new[]
    {
        new User(){Name = "Julian", Role = Role.User, IsVerification = true}
        
    });
    private int GetNextId() => Users.Count == 0 ? 0 : Users.Max(user => user.Id) + 1;

    [HttpGet]
    public IEnumerable<User> Get() => Users;

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var user = Users.SingleOrDefault(user => user.Id == id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    [HttpPost]
    public IActionResult Post(User user)
    {
        if (!ModelState.IsValid)
                return NotFound();
        user.Id = GetNextId();
        Users.Add(user);
        // context.User.Add(user);
        // context.SaveChanges();
        return CreatedAtAction(nameof(Get),new{id=user.Id}, user);

    }

    [HttpPut]
    public IActionResult Put(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var changedUser = Users.SingleOrDefault(storeUser => storeUser.Id == user.Id);
        if (changedUser == null)
            return NotFound();
        changedUser.Name = user.Name;
        changedUser.Role = user.Role;
        changedUser.IsVerification = user.IsVerification;
        changedUser.UserTrips = user.UserTrips;
        return Ok(changedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleteUser = Users.SingleOrDefault(storeUser => storeUser.Id == id);
        if (deleteUser == null)
            return BadRequest();
        Users.Remove(deleteUser);
        return Ok();
    }
}