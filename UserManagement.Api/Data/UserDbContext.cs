using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Entities;

namespace UserManagement.Api.Data;

/// <summary>
/// DbContext for User entity
/// </summary>
public class UserDbContext : DbContext
{
    /// <summary>
    /// User entity table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <inheritdoc />
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(new User
        {
            Guid = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
            Name = "FirstAdmin",
            Password = "$2a$12$cM50rnz.gJIlKfPwfl5gAu9zJRaZOp2cHn0cvOkQPTUwdscJ76yIG", //qwerty123
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
