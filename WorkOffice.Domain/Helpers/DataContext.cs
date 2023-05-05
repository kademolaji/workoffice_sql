using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        //NHS Setup
        public DbSet<NHSActivity> NHSActivities { get; set; }
        public DbSet<AppType> AppTypes { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<PathwayStatus> PathwayStatuses { get; set; }
        public DbSet<RTT> RTTs { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<WaitingType> WaitingTypes { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Department> Departments { get; set; }

        //NHS
        public DbSet<NHS_Appointment> NHS_Appointments { get; set; }
        public DbSet<NHS_Patient> NHS_Patients { get; set; }
        public DbSet<NHS_Waitinglist> NHS_Waitinglists { get; set; }
        public DbSet<NHS_Patientdocument> NHS_Patientdocuments { get; set; }
        public DbSet<NHS_Patient_Validation> NHS_Patient_Validations { get; set; }
        public DbSet<NHS_Patient_Validation_Detail> NHS_Patient_Validation_Details { get; set; }
        public DbSet<NHS_Diagnostic> NHS_Diagnostics { get; set; }
        public DbSet<NHS_DiagnosticResult> NHS_DiagnosticResults { get; set; }
        public DbSet<NHS_Referral> NHS_Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Here we create the indexes for each entity manually

            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.UserId });
            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.Email });
            modelBuilder.Entity<UserAccount>().HasIndex(u => new { u.FirstName, u.LastName });
            modelBuilder.Entity<UserAccountRole>().HasIndex(ur => new { ur.UserAccountRoleId });
            modelBuilder.Entity<Notification>().HasIndex(p => new { p.NotificationId });
            modelBuilder.Entity<AuditTrail>().HasIndex(p => new { p.AuditTrailId });
            modelBuilder.Entity<Location>().HasIndex(p => new { p.LocationId });
            modelBuilder.Entity<GeneralInformation>().HasIndex(p => new { p.GeneralInformationId });
            modelBuilder.Entity<CompanyStructure>().HasIndex(p => new { p.CompanyStructureId });
            modelBuilder.Entity<StructureDefinition>().HasIndex(p => new { p.StructureDefinitionId });
            modelBuilder.Entity<CustomIdentityFormatSetting>().HasIndex(p => new { p.CustomIdentityFormatSettingId });
            modelBuilder.Entity<UserAccess>().HasIndex(p => new { p.UserAccessId });
            modelBuilder.Entity<UserAccountAdditionalActivity>().HasIndex(p => new { p.UserAccountAdditionalActivityId });
            modelBuilder.Entity<UserAccountSettings>().HasIndex(p => new { p.UserAccountSettingsId });
            modelBuilder.Entity<UserActivity>().HasIndex(p => new { p.UserActivityId });
            modelBuilder.Entity<UserActivityParent>().HasIndex(p => new { p.UserActivityParentId });
            modelBuilder.Entity<UserRoleActivity>().HasIndex(p => new { p.UserRoleActivityId });
            modelBuilder.Entity<UserRoleDefinition>().HasIndex(p => new { p.UserRoleDefinitionId });
            modelBuilder.Entity<Country>().HasIndex(p => new { p.CountryId });
            modelBuilder.Entity<State>().HasIndex(p => new { p.StateId });


            modelBuilder.Entity<NHSActivity>().HasIndex(ur => new { ur.NHSActivityId });
            modelBuilder.Entity<AppType>().HasIndex(ur => new { ur.AppTypeId });
            modelBuilder.Entity<Consultant>().HasIndex(ur => new { ur.ConsultantId });
            modelBuilder.Entity<Hospital>().HasIndex(ur => new { ur.HospitalId });
            modelBuilder.Entity<PathwayStatus>().HasIndex(ur => new { ur.PathwayStatusId });
            modelBuilder.Entity<RTT>().HasIndex(ur => new { ur.RTTId });
            modelBuilder.Entity<Specialty>().HasIndex(ur => new { ur.SpecialtyId });
            modelBuilder.Entity<WaitingType>().HasIndex(ur => new { ur.WaitingTypeId });
            modelBuilder.Entity<Ward>().HasIndex(ur => new { ur.WardId });

            modelBuilder.Entity<NHS_Appointment>().HasIndex(ur => new { ur.AppointmentId });
            modelBuilder.Entity<NHS_Patient>().HasIndex(ur => new { ur.PatientId });
            modelBuilder.Entity<NHS_Waitinglist>().HasIndex(ur => new { ur.WaitinglistId });
            modelBuilder.Entity<NHS_Patientdocument>().HasIndex(ur => new { ur.PatientDocumentId });
            modelBuilder.Entity<NHS_Patient_Validation>().HasIndex(ur => new { ur.PatientValidationId });
            modelBuilder.Entity<NHS_Patient_Validation_Detail>().HasIndex(ur => new { ur.PatientValidationDetailsId });
            modelBuilder.Entity<NHS_Diagnostic>().HasIndex(ur => new { ur.DiagnosticId });
            modelBuilder.Entity<NHS_DiagnosticResult>().HasIndex(ur => new { ur.DiagnosticResultId });
            modelBuilder.Entity<NHS_Referral>().HasIndex(ur => new { ur.ReferralId });

            modelBuilder.UseIdentityColumns();

            FilterQuery(modelBuilder);
        }

        private void FilterQuery(ModelBuilder builder)
        {
            builder.Entity<Notification>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<AuditTrail>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Location>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<GeneralInformation>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<CompanyStructure>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<StructureDefinition>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<CustomIdentityFormatSetting>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserAccount>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserAccess>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserAccountAdditionalActivity>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserAccountRole>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserAccountSettings>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserActivity>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserActivityParent>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserRoleActivity>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<UserRoleDefinition>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Country>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<State>().HasQueryFilter(p => !p.IsDeleted);

            builder.Entity<NHSActivity>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<AppType>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Consultant>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Hospital>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<PathwayStatus>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<RTT>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Specialty>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<WaitingType>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Ward>().HasQueryFilter(p => !p.IsDeleted);

        }
    }
}
