using RapidRescue.Context;
using RapidRescue.Models;
using System;

namespace RapidRescue.Data.Seeders
{
    public static class DriverInfoSeeder
    {
        public static void SeedDrivers(RapidRescueContext context)
        {
            // Check if any drivers already exist
            if (!context.DriverInfo.Any())
            {
                // Seed driver with User_id = 3
                var drivers = new List<DriverInfo>
                {
                    new DriverInfo
                    {
                        PhoneNumber = "1234567890",
                        LicenseNumber = "LIC123456789",
                        LicenseExpiryDate = DateTime.UtcNow.AddYears(1), // License expiry one year from now
                        Address = "123 Main St, City, Country",
                        VehicleAssigned = "Ambulance-123",
                        DateOfHire = DateTime.UtcNow.AddMonths(-6), // Hired 6 months ago
                        IsActive = true, // Driver is active
                        User_id = 3, // Assuming this User_id 3 exists in the Users table
                        Latitude = 24.8736, // Example coordinates
                        Longitude = 67.0294,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                context.DriverInfo.AddRange(drivers);
                context.SaveChanges(); // Save changes to database
            }
        }
    }
}
