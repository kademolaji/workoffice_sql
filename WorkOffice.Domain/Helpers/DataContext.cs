using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkOffice.Domain.Entities.Account;
using WorkOffice.Domain.Entities.Shared;

namespace WorkOffice.Domain.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // Noop
        }

        // Account
        public DbSet<User> Users { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

       

        // Settings
        public DbSet<Country> Countries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here we create the indexes for each entity manually
            modelBuilder.Entity<User>().HasIndex(u => new { u.Id });
            modelBuilder.Entity<User>().HasIndex(u => new { u.Email });
            modelBuilder.Entity<User>().HasIndex(u => new { u.FirstName, u.LastName });
            modelBuilder.UseIdentityColumns();
        }
    }
}
