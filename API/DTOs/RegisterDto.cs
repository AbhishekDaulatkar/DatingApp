using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto //Data Transfer Objects - used to transfer data between differnt subsystems
{
  [Required]
  public string Username { get; set; }
  
  [Required]
  public string Password { get; set; }

}
