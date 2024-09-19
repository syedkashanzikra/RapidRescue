using Microsoft.AspNetCore.Identity;
using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Data.Seeders
{
    public static class UsersSeeder
    {
        public static void SeedUsers(RapidRescueContext context)
        {
            if (!context.Users.Any()) 
            {
                var passwordHasher = new PasswordHasher<Users>();

                var users = new List<Users>
                {
                    new Users
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        Password = passwordHasher.HashPassword(null, "admin123"), 
                        Role_Id = 1,  
                        IsActive = true,
                        RememberToken = "admin"
                    },
                    new Users
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "patient@example.com",
                        Password = passwordHasher.HashPassword(null, "patient123"), 
                        Role_Id = 2,  
                        IsActive = true,
                        RememberToken = "patient"
                    },
                    new Users
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "driver@example.com",
                        Password = passwordHasher.HashPassword(null, "driver123"), 
                        Role_Id = 3, 
                        IsActive = true,
                        RememberToken = "Driver"
                    },
                    new Users
                    {
                        FirstName = "Michael",
                        LastName = "Johnson",
                        Email = "emt@example.com",
                        Password = passwordHasher.HashPassword(null, "emt123"), 
                        Role_Id = 4,  
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
