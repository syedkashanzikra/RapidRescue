using RapidRescue.Context;
using RapidRescue.Models;

namespace RapidRescue.Data.Seeders
{
    public static class RolesSeeder
    {
        public static void SeedRoles(RapidRescueContext context)
        {
            if (!context.Roles.Any()) // Check if any roles already exist
            {
                var roles = new List<Roles>
                {
                    new Roles {  RoleName = "Admin", Status = true },
                    new Roles {  RoleName = "Patients", Status = true },
                    new Roles {  RoleName = "Drivers", Status = true },
                    new Roles {  RoleName = "EMT", Status = true },
                    


                };

                context.Roles.AddRange(roles);
                context.SaveChanges(); 
            }
        }
    }
}