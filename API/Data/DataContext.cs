using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    //
    public DataContext(DbContextOptions options) : base(options){} //base  = DbContext class

    public DbSet<AppUser> Users {get; set;} //AppUser -Entity Name; Users - Table name
}
