using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            Name = "FirstAdmin",
            Password = "qwerty123",
            Login = "Admin01",
            Gender = 2,
            Admin = true,
            CreatedOn = DateTime.Now,
            CreatedBy = "System",
            ModifiedBy = String.Empty,
            RevokedBy = String.Empty,
        });
    }
}   
