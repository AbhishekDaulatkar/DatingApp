using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512(); 
        //keyword 'using' is used to call Dispose method which unallocates unmanaged resources saving memroy and improving performance

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username); //put the table user into user variable after matching it with the user name from the post
          //now user contains both username and password; later we compare them with computedhash
        if (user == null) return Unauthorized("Invalid Username/NULL Username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var Computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < Computedhash.Length; i++)
        {
            if (user.PasswordHash[i] != Computedhash[i])
            {
                return Unauthorized("Invalid Password ");
            }
        }
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username){
    return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
