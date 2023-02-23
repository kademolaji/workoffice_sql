using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkOffice.Domain.Entities.Account;
using WorkOffice.Domain.Entities.Admin;
using WorkOffice.Domain.Entities.NHS_Setup;
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

       // Admin
       public DbSet<UserRole> UserRoles { get; set; }

        //NHS Setup
        public DbSet<Entities.NHS_Setup.Activity> Activities { get; set; }
        public DbSet<AppType> AppTypes { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<PathwayStatus> PathwayStatuses { get; set; }
        public DbSet<RTT> RTTs { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<WaitingType> WaitingTypes { get; set; }
        public DbSet<Ward> Wards { get; set; }

        // Settings
        public DbSet<Country> Countries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here we create the indexes for each entity manually
            modelBuilder.Entity<User>().HasIndex(u => new { u.UserId });
            modelBuilder.Entity<User>().HasIndex(u => new { u.Email });
            modelBuilder.Entity<User>().HasIndex(u => new { u.FirstName, u.LastName });
            modelBuilder.Entity<UserRole>().HasIndex(ur => new { ur.USerRoleId });
            modelBuilder.Entity<Entities.NHS_Setup.Activity>().HasIndex(ur => new { ur.ActivityId });
            modelBuilder.Entity<AppType>().HasIndex(ur => new { ur.AppTypeId });
            modelBuilder.Entity<Consultant>().HasIndex(ur => new { ur.ConsultantId });
            modelBuilder.Entity<Hospital>().HasIndex(ur => new { ur.HospitalId });
            modelBuilder.Entity<PathwayStatus>().HasIndex(ur => new { ur.PathwayStatusId });
            modelBuilder.Entity<RTT>().HasIndex(ur => new { ur.RTTId });
            modelBuilder.Entity<Specialty>().HasIndex(ur => new { ur.SpecialtyId });
            modelBuilder.Entity<WaitingType>().HasIndex(ur => new { ur.WaitingTypeId });
            modelBuilder.Entity<Ward>().HasIndex(ur => new { ur.WardId });
            modelBuilder.UseIdentityColumns();
        }
    }
}
