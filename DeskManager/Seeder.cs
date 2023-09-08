using DeskManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace DeskManager;

public class Seeder
{
    private readonly DeskManagerDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public Seeder(DeskManagerDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
            if (!_dbContext.Users.Any())
            {
                var users = GetUsers();
                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Locations.Any())
        {
            var locations = GetLocations();
            _dbContext.Locations.AddRange(locations);
            _dbContext.SaveChanges();
        }
    }

    private IEnumerable<User> GetUsers()
    {
        var list = new List<User>();
        var admin = new User()
        {
            Email = "admin@admin.com",
            FirstName = "admin",
            LastName = "admin",
            RoleId = 1,
        };
        var password = "adminPassword";
        var hashedPassword = _passwordHasher.HashPassword(admin, password);
        admin.Password = hashedPassword;
        
        var user = new User()
        {
            Email = "user@user.com",
            FirstName = "user",
            LastName = "user",
            RoleId = 2,
        };
        var passwordUser = "userPassword";
        var hashedPasswordUser = _passwordHasher.HashPassword(user, passwordUser);
        user.Password = hashedPasswordUser;
        list.Add(admin);
        list.Add(user);
        return list;
    }

    private List<Location> GetLocations()
    {
        var list = new List<Location>();
        var location1 = new Location()
        {
            City = "Katowice",
            Desks = { new Desk() { IsAvailable = true, }, new Desk() { IsAvailable = true } },
            Name = "Rio",
            Room = "12",
            Street = "Korfantego",
        };
        var location2 = new Location()
        {
            City = "Kraków",
            Desks = { new Desk() { IsAvailable = false, }, new Desk() { IsAvailable = true } },
            Name = "Piwnica",
            Room = "P123",
            Street = "Tuwima",
        };
        list.Add(location1);
        list.Add(location2);
        return list;
    }
}