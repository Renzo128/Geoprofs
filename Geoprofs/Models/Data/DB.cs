using Microsoft.EntityFrameworkCore;
using Geoprofs.Models;

namespace Geoprofs.Models.Data
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {
        }

        public DbSet<AbsenceRequest> absenceRequests { get; set; } // database maken
        public DbSet<AbsenceType> absenceTypes { get; set; }
        public DbSet<Coworker> coworkers { get; set; }
        public DbSet<Login> logins { get; set; }
        public DbSet<Position> positions { get; set; }
        public DbSet<Supervisor> supervisors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsenceRequest>().ToTable("AbsenceRequest");
            modelBuilder.Entity<AbsenceType>().ToTable("AbsenceType");
            modelBuilder.Entity<Coworker>().ToTable("Coworker");
            modelBuilder.Entity<Login>().ToTable("Login");
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Supervisor>().ToTable("Supervisor");

        }
    }


}


