using Blabalacar.Database;
using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;
[Route("[controller]")]
public class UserController: Controller
{
    private readonly BlalacarContext _context;

    public UserController(BlalacarContext context)
    {
        _context = context;
    }
    private int GetNextId() => _context.User.Local.Count == 0 ? 0 : _context.User.Local.Max(user => user.Id) + 1;

    [HttpGet]
    public IEnumerable<User> Get() => _context.User;
    
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var user = _context.User.SingleOrDefault(user => user.Id == id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    [HttpPost]
    public IActionResult Post(CreateUserBody createUserBody)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var user = new User(GetNextId(), createUserBody.Name, createUserBody.Role, createUserBody.IsVerification);
        _context.User.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get),new{id=user.Id}, user);
    }

    [HttpPut]
    public IActionResult Put(User user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var changedUser = _context.User.SingleOrDefault(storeUser => storeUser.Id == user.Id);
        if (changedUser == null)
            return NotFound();
        changedUser.Name = user.Name;
        changedUser.Role = user.Role;
        changedUser.IsVerification = user.IsVerification;
        changedUser.UserTrips = user.UserTrips;
        _context.SaveChanges();
        return Ok(changedUser);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleteUser = _context.User.SingleOrDefault(storeUser => storeUser.Id == id);
        if (deleteUser == null)
            return BadRequest();
        _context.User.Remove(deleteUser);
        _context.SaveChanges();
        return Ok();
    }
}