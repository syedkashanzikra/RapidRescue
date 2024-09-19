using Microsoft.EntityFrameworkCore;
using RapidRescue.Models;
using System.Data;

namespace RapidRescue.Context
{
  
    public class RapidRescueContext : DbContext
    {
        public RapidRescueContext(DbContextOptions<RapidRescueContext> options)
            : base(options)
        {

        }

      public  DbSet<Roles> Roles { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<PatientsInfo> PatientsInfo { get; set; }

        public DbSet<DriverInfo> DriverInfo { get; set; }


    }
}
