using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;

namespace DeskManager.Entities;

public class DeskManagerDbContext : DbContext
{
    public DeskManagerDbContext(DbContextOptions<DeskManagerDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasData(
                new Role() { Id=1 , RoleName = "Admin" }, 
                new Role() { Id=2 , RoleName = "User" });
        
    }
}