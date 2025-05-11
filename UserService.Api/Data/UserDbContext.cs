using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Api.Authentication;
using UserService.Api.Entities;

namespace UserService.Api.Data;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(new User
        {
            Guid = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
            Name = "FirstAdmin",
            Password = "$2a$12$cM50rnz.gJIlKfPwfl5gAu9zJRaZOp2cHn0cvOkQPTUwdscJ76yIG",
            Login = "Admin01",
            Gender = 2,
            Admin = true,
            CreatedOn = DateTime.MinValue,
            CreatedBy = "System",
            ModifiedBy = String.Empty,
            ModifiedOn = DateTime.MinValue,
        });
    }
}   
