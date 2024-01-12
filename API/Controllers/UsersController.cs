using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controller;
 //takes first name of the controller - /api/users
 [Authorize]
public class UsersController : BaseApiController // ControllerBase Class - used for MVC design pattern
{
    private readonly DataContext _context;

    public UsersController(DataContext context) //Dependency Injection - using constructor 
    {
        _context = context;
    }
    [AllowAnonymous]
    [HttpGet] //endpoints

    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() //used Task<> for async and await
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")] //endpoints -/api/users/1
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var users = await _context.Users.FindAsync(id);
        return users;
    }
}
