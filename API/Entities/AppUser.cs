namespace API.Entities //used naming as API.Entities as in conventional way; will use the same always
{
public class AppUser //Id and UserName will be columns in DB.
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public byte[] PasswordHash {get; set;}

    public byte[] PasswordSalt { get; set; }
}
}
