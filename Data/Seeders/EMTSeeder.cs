using RapidRescue.Context;
using RapidRescue.Models;
using System;

namespace RapidRescue.Data.Seeders
{
    public static class EMTSeeder
    {
        public static void SeedEMTs(RapidRescueContext context)
        {
            // Check if any EMTs already exist
            if (!context.EMTs.Any())
            {
                // Seed EMT with User_id = 4
                var emts = new List<EMT>
                {
                    new EMT
                    {
                        User_id = 4, // Assuming this User_id 4 exists in the Users table
                        CertificationNumber = "CERT123456",
                        CertificationExpiryDate = DateTime.UtcNow.AddYears(1), // Certification expiry one year from now
                        ContactNumber = "9876543210",
                        LicenseNumber = "LIC789456123",
                        IsAvailable = true, // EMT is available
                        Address = "789 Oak St, City, Country",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.EMTs.AddRange(emts);
                context.SaveChanges(); // Save changes to database
            }
        }
    }
}
