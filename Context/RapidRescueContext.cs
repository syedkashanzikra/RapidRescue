using Microsoft.EntityFrameworkCore;
using RapidRescue.Models;

namespace RapidRescue.Context
{
    public class RapidRescueContext : DbContext
    {
        public RapidRescueContext(DbContextOptions<RapidRescueContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<PatientsInfo> PatientsInfo { get; set; }
        public DbSet<DriverInfo> DriverInfo { get; set; }
        public DbSet<EMT> EMTs { get; set; }

        public DbSet<Ambulance> Ambulances { get; set; }



    }
}
