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
       
        
    }
}
