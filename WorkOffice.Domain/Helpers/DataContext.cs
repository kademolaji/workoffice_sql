using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StructureDefinition>().HasIndex(x => x.Definition).IsUnique();
            // Here we create the indexes for each entity manually

            FilterQuery(modelBuilder);
        }

        private void FilterQuery(ModelBuilder builder)
        {
            builder.Entity<AuditTrail>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<StructureDefinition>().HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
