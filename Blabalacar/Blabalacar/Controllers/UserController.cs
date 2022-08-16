using Blabalacar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blabalacar.Controllers;

public class UserController:Controller
{
    private List<User> Users = new List<User>();
    private int GetNextId() => Users.Count == 0 ? 0 : Users.Max(user => user.Id) + 1;

    [HttpGet]
    public IEnumerable<User> Get() => Users;

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = Users.SingleOrDefault(user => user.Id == id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }
    [HttpPost]
    public IActionResult Post([FromBody] User user)
    {
        if (!ModelState.IsValid)
            return NotFound();
        user.Id = GetNextId();
        Users.Add(user);
        return CreatedAtAction(nameof(Get),new{id=user.Id}, user);
    }
    
}