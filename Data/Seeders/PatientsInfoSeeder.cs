using RapidRescue.Context;
using RapidRescue.Models;
using System;

namespace RapidRescue.Data.Seeders
{
    public static class PatientsInfoSeeder
    {
        public static void SeedPatients(RapidRescueContext context)
        {
            // Check if any patients already exist
            if (!context.PatientsInfo.Any())
            {
                // Seed patient with User_id = 2
                var patients = new List<PatientsInfo>
                {
                    new PatientsInfo
                    {
                        User_id = 2, // Assuming this User_id 2 exists in the Users table
                        MobileNumber = "9876543210",
                        Situation = "High fever and difficulty breathing",
                        PickupLocation = "456 Elm St, City, Country",
                        RequestedTime = DateTime.UtcNow, // Time of request
                        AdditionalDetails = "Patient has had symptoms for 3 days.",
                        Gender = "Male",
                        IsEmergency = true // Mark this as an emergency request
                    }
                };

                context.PatientsInfo.AddRange(patients);
                context.SaveChanges(); // Save changes to database
            }
        }
    }
}
