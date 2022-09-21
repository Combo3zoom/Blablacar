using Blabalacar.Database;
using Blabalacar.Models;
using Blabalacar.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blabalacar.Controllers;
[Route("[controller]")]
[Authorize]
public class UserController: Controller
{
    private readonly BlalacarContext _context;

    public UserController(BlalacarContext context)
    {
        _context = context;
    }
    private int GetNextId() => _context.User.Local.Count == 0 ? 0 : _context.User.Local.Max(user => user.Id) + 1;

    [HttpGet, AllowAnonymous]
    public IEnumerable<User> Get() => _context.User.Include("UserTrips");
    
    [HttpGet("{id:int}"), AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var user = _context.User.Include("UserTrips").SingleOrDefault(user => user.Id == id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    [HttpHead, AllowAnonymous]
    public IEnumerable<User> Head() => _context.User.Include("UserTrips");
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserBody createUserBody)
    {
        if (!ModelState.IsValid)
            return NotFound();
        var user = new User(GetNextId(), createUserBody.Name);
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get),new{id=user.Id}, user);
    }

    [HttpPut]
    public async Task<IActionResult> Put(User user)
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
        await _context.SaveChangesAsync();
        return Ok(changedUser);
    }

    [HttpDelete("{id:int}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteUser = _context.User.SingleOrDefault(storeUser => storeUser.Id == id);
        if (deleteUser == null)
            return BadRequest();
        _context.User.Remove(deleteUser);
        await _context.SaveChangesAsync();
        return Ok();
    }
}