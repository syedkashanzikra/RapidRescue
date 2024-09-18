using Microsoft.CodeAnalysis.Scripting;
using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Data.Seeders
{
    public static class UsersSeeder
    {
        public static void SeedUsers(RapidRescueContext context)
        {
            if (!context.Users.Any()) // Check if any users already exist
            {
                var users = new List<Users>
                {
                    new Users
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin123"), // Hashing password
                        Role_Id = 1,  // Admin Role
                        IsActive = true,
                        RememberToken = "admin"
                    },
                    new Users
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "patient@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("patient123"), // Hashing password
                        Role_Id = 2,  // Patient Role
                        IsActive = true,
                        RememberToken = "patient"
                    },
                    new Users
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "driver@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("driver123"), // Hashing password
                        Role_Id = 3,  // Driver Role
                        IsActive = true,
                        RememberToken = "Driver"
                    },
                    new Users
                    {
                        FirstName = "Michael",
                        LastName = "Johnson",
                        Email = "emt@example.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("emt123"), // Hashing password
                        Role_Id = 4,  // EMT Role
                        IsActive = true,
                        RememberToken = "EMT"
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
