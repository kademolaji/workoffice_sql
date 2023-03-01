using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WorkOffice.Domain.Entities.NHS_Setup;

using WorkOffice.Domain.Entities;


namespace WorkOffice.Domain.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // Noop
        }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<GeneralInformation> GeneralInformations { get; set; }
        public virtual DbSet<CompanyStructure> CompanyStructures { get; set; }
        public virtual DbSet<StructureDefinition> StructureDefinitions { get; set; }
        public virtual DbSet<CustomIdentityFormatSetting> CustomIdentityFormatSettings { get; set; }

        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<UserAccess> UserAccesses { get; set; }
        public virtual DbSet<UserAccountAdditionalActivity> UserAccountAdditionalActivities { get; set; }
        public virtual DbSet<UserAccountRole> UserAccountRoles { get; set; }
        public virtual DbSet<UserAccountSettings> UserAccountSettings { get; set; }
        public virtual DbSet<UserActivity> UserActivities { get; set; }
        public virtual DbSet<UserActivityParent> UserActivityParents { get; set; }
        public virtual DbSet<UserRoleActivity> UserRoleActivities { get; set; }
        public virtual DbSet<UserRoleDefinition> UserRoleDefinitions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<State> States { get; set; }


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
        //public DbSet<Country> Countries { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StructureDefinition>().HasIndex(x => x.Definition).IsUnique();
            // Here we create the indexes for each entity manually

            //modelBuilder.Entity<User>().HasIndex(u => new { u.UserId });
            //modelBuilder.Entity<User>().HasIndex(u => new { u.Email });
            //modelBuilder.Entity<User>().HasIndex(u => new { u.FirstName, u.LastName });
            //modelBuilder.Entity<UserRole>().HasIndex(ur => new { ur.USerRoleId });
            
            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.UserId });
            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.Email });
            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.FirstName, u.LastName });
            modelBuilder.Entity<UserAccountRole>().HasIndex(ur => new { ur.UserAccountRoleId });
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

            FilterQuery(modelBuilder);
        }

        private void FilterQuery(ModelBuilder builder)
        {
            builder.Entity<AuditTrail>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<StructureDefinition>().HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
