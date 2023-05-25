using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkOffice.Domain.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTypes",
                columns: table => new
                {
                    AppTypeId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypes", x => x.AppTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    AuditTrailId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ActionDate = table.Column<DateTime>(nullable: true),
                    ActionBy = table.Column<string>(maxLength: 50, nullable: true),
                    Details = table.Column<string>(maxLength: 2000, nullable: true),
                    IPAddress = table.Column<string>(maxLength: 100, nullable: true),
                    HostAddress = table.Column<string>(maxLength: 200, nullable: true),
                    Page = table.Column<string>(maxLength: 100, nullable: true),
                    ActionType = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.AuditTrailId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsAfrica = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "CustomIdentityFormatSettings",
                columns: table => new
                {
                    CustomIdentityFormatSettingId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    Digits = table.Column<int>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    Separator = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomIdentityFormatSettings", x => x.CustomIdentityFormatSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "GeneralInformations",
                columns: table => new
                {
                    GeneralInformationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Organisationname = table.Column<string>(nullable: true),
                    Taxid = table.Column<string>(nullable: true),
                    Regno = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Zipcode = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    ImgLogo = table.Column<byte[]>(nullable: true),
                    Imgtype = table.Column<string>(nullable: true),
                    Ismulticompany = table.Column<bool>(nullable: false),
                    Subsidiary_level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInformations", x => x.GeneralInformationId);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_DiagnosticResults",
                columns: table => new
                {
                    DiagnosticResultId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiagnosticId = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    Problem = table.Column<string>(maxLength: 500, nullable: true),
                    ConsultantName = table.Column<string>(maxLength: 255, nullable: false),
                    DocumentName = table.Column<string>(maxLength: 255, nullable: false),
                    DocumentExtension = table.Column<string>(maxLength: 50, nullable: false),
                    DocumentFile = table.Column<byte[]>(nullable: false),
                    TestResultDate = table.Column<DateTime>(nullable: true),
                    DateUploaded = table.Column<DateTime>(nullable: true),
                    SpecialityId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_DiagnosticResults", x => x.DiagnosticResultId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Diagnostics",
                columns: table => new
                {
                    DiagnosticId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(nullable: false),
                    SpecialtyId = table.Column<int>(nullable: false),
                    Problem = table.Column<string>(maxLength: 550, nullable: false),
                    DTD = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    ConsultantName = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Diagnostics", x => x.DiagnosticId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Patient_Validation_Details",
                columns: table => new
                {
                    PatientValidationDetailsId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientValidationId = table.Column<int>(nullable: false),
                    PathWayStatusId = table.Column<int>(nullable: true),
                    SpecialityId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ConsultantId = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PatientId = table.Column<int>(nullable: true),
                    Activity = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Type = table.Column<string>(maxLength: 50, nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patient_Validation_Details", x => x.PatientValidationDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Patient_Validations",
                columns: table => new
                {
                    PatientValidationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(nullable: false),
                    PathWayNumber = table.Column<string>(maxLength: 150, nullable: false),
                    PathWayCondition = table.Column<string>(maxLength: 150, nullable: true),
                    PathWayStatusId = table.Column<int>(nullable: true),
                    PathWayStatusIdCode = table.Column<string>(nullable: true),
                    PathWayStartDate = table.Column<DateTime>(nullable: false),
                    PathWayEndDate = table.Column<DateTime>(nullable: true),
                    SpecialtyId = table.Column<int>(nullable: false),
                    RTTId = table.Column<int>(maxLength: 50, nullable: false),
                    RTTWait = table.Column<string>(maxLength: 550, nullable: true),
                    DistrictNumber = table.Column<string>(maxLength: 50, nullable: true),
                    NHSNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patient_Validations", x => x.PatientValidationId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Patientdocuments",
                columns: table => new
                {
                    PatientDocumentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentTypeId = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    PhysicalLocation = table.Column<string>(maxLength: 500, nullable: true),
                    DocumentName = table.Column<string>(maxLength: 255, nullable: false),
                    DocumentExtension = table.Column<string>(maxLength: 50, nullable: false),
                    DocumentFile = table.Column<byte[]>(nullable: false),
                    ClinicDate = table.Column<DateTime>(nullable: true),
                    DateUploaded = table.Column<DateTime>(nullable: true),
                    SpecialityId = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patientdocuments", x => x.PatientDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Referrals",
                columns: table => new
                {
                    ReferralId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(nullable: false),
                    SpecialtyId = table.Column<int>(nullable: false),
                    ConsultantId = table.Column<int>(nullable: false),
                    ReferralDate = table.Column<DateTime>(nullable: false),
                    ConsultantName = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentExtension = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentName = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentFile = table.Column<byte[]>(nullable: false),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Referrals", x => x.ReferralId);
                });

            migrationBuilder.CreateTable(
                name: "NHSActivities",
                columns: table => new
                {
                    NHSActivityId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHSActivities", x => x.NHSActivityId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SenderId = table.Column<long>(nullable: false),
                    ReceiverId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "PathwayStatuses",
                columns: table => new
                {
                    PathwayStatusId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    AllowClosed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PathwayStatuses", x => x.PathwayStatusId);
                });

            migrationBuilder.CreateTable(
                name: "RTTs",
                columns: table => new
                {
                    RTTId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTTs", x => x.RTTId);
                });

            migrationBuilder.CreateTable(
                name: "StructureDefinitions",
                columns: table => new
                {
                    StructureDefinitionId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Definition = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructureDefinitions", x => x.StructureDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomUserCode = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ProfilePicture = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Biography = table.Column<string>(nullable: true),
                    Disabled = table.Column<bool>(nullable: false),
                    DeletionDate = table.Column<DateTime>(nullable: false),
                    AcceptTerms = table.Column<bool>(nullable: false),
                    VerificationToken = table.Column<string>(nullable: true),
                    Verified = table.Column<DateTime>(nullable: true),
                    ResetToken = table.Column<string>(nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(nullable: true),
                    PasswordReset = table.Column<DateTime>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    IsFirstLoginAttempt = table.Column<bool>(nullable: false),
                    SecurityQuestion = table.Column<string>(maxLength: 256, nullable: true),
                    SecurityAnswer = table.Column<string>(maxLength: 256, nullable: true),
                    NextPasswordChangeDate = table.Column<DateTime>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    LastActive = table.Column<DateTime>(nullable: true),
                    CurrentLogin = table.Column<DateTime>(nullable: true),
                    CanChangePassword = table.Column<bool>(nullable: true),
                    Accesslevel = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountSettings",
                columns: table => new
                {
                    UserAccountSettingsId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EnablePasswordReset = table.Column<bool>(nullable: false),
                    MinimumRequiredPasswordLength = table.Column<int>(nullable: true),
                    MaximumInvalidPasswordAttempts = table.Column<int>(nullable: true),
                    AllowPasswordUserAfter = table.Column<int>(nullable: true),
                    ExpirePasswordAfter = table.Column<int>(nullable: true),
                    MaxPeriodOfUserInactivity = table.Column<int>(nullable: true),
                    SessionTimeout = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountSettings", x => x.UserAccountSettingsId);
                });

            migrationBuilder.CreateTable(
                name: "UserActivityParents",
                columns: table => new
                {
                    UserActivityParentId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserActivityParentName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityParents", x => x.UserActivityParentId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleDefinitions",
                columns: table => new
                {
                    UserRoleDefinitionId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleDefinitions", x => x.UserRoleDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "WaitingTypes",
                columns: table => new
                {
                    WaitingTypeId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingTypes", x => x.WaitingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultants",
                columns: table => new
                {
                    ConsultantId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NHS_ReferralReferralId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultants", x => x.ConsultantId);
                    table.ForeignKey(
                        name: "FK_Consultants_NHS_Referrals_NHS_ReferralReferralId",
                        column: x => x.NHS_ReferralReferralId,
                        principalTable: "NHS_Referrals",
                        principalColumn: "ReferralId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DistrictNumber = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 150, nullable: false),
                    LastName = table.Column<string>(maxLength: 150, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 150, nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 550, nullable: true),
                    PhoneNo = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Sex = table.Column<string>(maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    NHSNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    NHS_DiagnosticDiagnosticId = table.Column<int>(nullable: true),
                    NHS_ReferralReferralId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_NHS_Patients_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                        column: x => x.NHS_DiagnosticDiagnosticId,
                        principalTable: "NHS_Diagnostics",
                        principalColumn: "DiagnosticId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Patients_NHS_Referrals_NHS_ReferralReferralId",
                        column: x => x.NHS_ReferralReferralId,
                        principalTable: "NHS_Referrals",
                        principalColumn: "ReferralId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    SpecialtyId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NHS_DiagnosticDiagnosticId = table.Column<int>(nullable: true),
                    NHS_ReferralReferralId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.SpecialtyId);
                    table.ForeignKey(
                        name: "FK_Specialties_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                        column: x => x.NHS_DiagnosticDiagnosticId,
                        principalTable: "NHS_Diagnostics",
                        principalColumn: "DiagnosticId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Specialties_NHS_Referrals_NHS_ReferralReferralId",
                        column: x => x.NHS_ReferralReferralId,
                        principalTable: "NHS_Referrals",
                        principalColumn: "ReferralId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStructures",
                columns: table => new
                {
                    CompanyStructureId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    StructureTypeID = table.Column<long>(maxLength: 50, nullable: false),
                    Country = table.Column<string>(maxLength: 250, nullable: true),
                    Parent = table.Column<string>(maxLength: 250, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    CompanyHead = table.Column<string>(maxLength: 100, nullable: true),
                    ParentID = table.Column<long>(nullable: true),
                    Company = table.Column<string>(maxLength: 250, nullable: true),
                    StructureDefinitionId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStructures", x => x.CompanyStructureId);
                    table.ForeignKey(
                        name: "FK_CompanyStructures_StructureDefinitions_StructureDefinitionId",
                        column: x => x.StructureDefinitionId,
                        principalTable: "StructureDefinitions",
                        principalColumn: "StructureDefinitionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokenId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountUserId = table.Column<long>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_UserAccounts_UserAccountUserId",
                        column: x => x.UserAccountUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    UserActivityId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserActivityName = table.Column<string>(maxLength: 256, nullable: false),
                    UserActivityParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.UserActivityId);
                    table.ForeignKey(
                        name: "FK_UserActivities_UserActivityParents_UserActivityParentId",
                        column: x => x.UserActivityParentId,
                        principalTable: "UserActivityParents",
                        principalColumn: "UserActivityParentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountRoles",
                columns: table => new
                {
                    UserAccountRoleId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountId = table.Column<long>(nullable: false),
                    UserRoleDefinitionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountRoles", x => x.UserAccountRoleId);
                    table.ForeignKey(
                        name: "FK_UserAccountRoles_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccountRoles_UserRoleDefinitions_UserRoleDefinitionId",
                        column: x => x.UserRoleDefinitionId,
                        principalTable: "UserRoleDefinitions",
                        principalColumn: "UserRoleDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppTypeId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    SpecialityId = table.Column<int>(nullable: true),
                    BookDate = table.Column<DateTime>(nullable: false),
                    AppDate = table.Column<DateTime>(nullable: false),
                    AppTime = table.Column<string>(maxLength: 50, nullable: true),
                    ConsultantId = table.Column<int>(nullable: true),
                    HospitalId = table.Column<int>(nullable: true),
                    WardId = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    PatientId = table.Column<int>(nullable: false),
                    PatientValidationId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    AppointmentStatus = table.Column<string>(maxLength: 50, nullable: true),
                    CancellationReason = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    NHS_PatientPatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_NHS_Patients_NHS_PatientPatientId",
                        column: x => x.NHS_PatientPatientId,
                        principalTable: "NHS_Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NHS_Waitinglists",
                columns: table => new
                {
                    WaitinglistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WaitTypeId = table.Column<int>(nullable: false),
                    SpecialityId = table.Column<int>(nullable: false),
                    TCIDate = table.Column<DateTime>(nullable: true),
                    WaitinglistDate = table.Column<DateTime>(nullable: false),
                    WaitinglistTime = table.Column<string>(maxLength: 50, nullable: true),
                    PatientId = table.Column<int>(nullable: true),
                    patientValidationId = table.Column<int>(nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    WaitinglistStatus = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    NHS_PatientPatientId = table.Column<int>(nullable: true),
                    NHS_Patient_ValidationPatientValidationId = table.Column<int>(nullable: true),
                    SpecialtyId = table.Column<long>(nullable: true),
                    WaitingTypeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Waitinglists", x => x.WaitinglistId);
                    table.ForeignKey(
                        name: "FK_NHS_Waitinglists_NHS_Patients_NHS_PatientPatientId",
                        column: x => x.NHS_PatientPatientId,
                        principalTable: "NHS_Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Waitinglists_NHS_Patient_Validations_NHS_Patient_Valida~",
                        column: x => x.NHS_Patient_ValidationPatientValidationId,
                        principalTable: "NHS_Patient_Validations",
                        principalColumn: "PatientValidationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Waitinglists_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "SpecialtyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Waitinglists_WaitingTypes_WaitingTypeId",
                        column: x => x.WaitingTypeId,
                        principalTable: "WaitingTypes",
                        principalColumn: "WaitingTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAccesses",
                columns: table => new
                {
                    UserAccessId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CompanyStructureId = table.Column<long>(nullable: false),
                    UserAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccesses", x => x.UserAccessId);
                    table.ForeignKey(
                        name: "FK_UserAccesses_CompanyStructures_CompanyStructureId",
                        column: x => x.CompanyStructureId,
                        principalTable: "CompanyStructures",
                        principalColumn: "CompanyStructureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccesses_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccountAdditionalActivities",
                columns: table => new
                {
                    UserAccountAdditionalActivityId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountId = table.Column<long>(nullable: false),
                    UserActivityId = table.Column<long>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: true),
                    CanAdd = table.Column<bool>(nullable: true),
                    CanView = table.Column<bool>(nullable: true),
                    CanDelete = table.Column<bool>(nullable: true),
                    CanApprove = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountAdditionalActivities", x => x.UserAccountAdditionalActivityId);
                    table.ForeignKey(
                        name: "FK_UserAccountAdditionalActivities_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccountAdditionalActivities_UserActivities_UserActivity~",
                        column: x => x.UserActivityId,
                        principalTable: "UserActivities",
                        principalColumn: "UserActivityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleActivities",
                columns: table => new
                {
                    UserRoleActivityId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserRoleDefinitionId = table.Column<long>(nullable: false),
                    UserActivityId = table.Column<long>(nullable: false),
                    CanEdit = table.Column<bool>(nullable: true),
                    CanAdd = table.Column<bool>(nullable: true),
                    CanView = table.Column<bool>(nullable: true),
                    CanDelete = table.Column<bool>(nullable: true),
                    CanApprove = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleActivities", x => x.UserRoleActivityId);
                    table.ForeignKey(
                        name: "FK_UserRoleActivities_UserActivities_UserActivityId",
                        column: x => x.UserActivityId,
                        principalTable: "UserActivities",
                        principalColumn: "UserActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleActivities_UserRoleDefinitions_UserRoleDefinitionId",
                        column: x => x.UserRoleDefinitionId,
                        principalTable: "UserRoleDefinitions",
                        principalColumn: "UserRoleDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTypes_AppTypeId",
                table: "AppTypes",
                column: "AppTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_AuditTrailId",
                table: "AuditTrails",
                column: "AuditTrailId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_CompanyStructureId",
                table: "CompanyStructures",
                column: "CompanyStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_StructureDefinitionId",
                table: "CompanyStructures",
                column: "StructureDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_ConsultantId",
                table: "Consultants",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_NHS_ReferralReferralId",
                table: "Consultants",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryId",
                table: "Countries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomIdentityFormatSettings_CustomIdentityFormatSettingId",
                table: "CustomIdentityFormatSettings",
                column: "CustomIdentityFormatSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_GeneralInformationId",
                table: "GeneralInformations",
                column: "GeneralInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_HospitalId",
                table: "Hospitals",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationId",
                table: "Locations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_AppointmentId",
                table: "NHS_Appointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_NHS_PatientPatientId",
                table: "NHS_Appointments",
                column: "NHS_PatientPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_DiagnosticResults_DiagnosticResultId",
                table: "NHS_DiagnosticResults",
                column: "DiagnosticResultId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Diagnostics_DiagnosticId",
                table: "NHS_Diagnostics",
                column: "DiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validation_Details_PatientValidationDetailsId",
                table: "NHS_Patient_Validation_Details",
                column: "PatientValidationDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validations_PatientValidationId",
                table: "NHS_Patient_Validations",
                column: "PatientValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patientdocuments_PatientDocumentId",
                table: "NHS_Patientdocuments",
                column: "PatientDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients",
                column: "NHS_DiagnosticDiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_NHS_ReferralReferralId",
                table: "NHS_Patients",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_PatientId",
                table: "NHS_Patients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Referrals_ReferralId",
                table: "NHS_Referrals",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Waitinglists_NHS_PatientPatientId",
                table: "NHS_Waitinglists",
                column: "NHS_PatientPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Waitinglists_NHS_Patient_ValidationPatientValidationId",
                table: "NHS_Waitinglists",
                column: "NHS_Patient_ValidationPatientValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Waitinglists_SpecialtyId",
                table: "NHS_Waitinglists",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Waitinglists_WaitingTypeId",
                table: "NHS_Waitinglists",
                column: "WaitingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Waitinglists_WaitinglistId",
                table: "NHS_Waitinglists",
                column: "WaitinglistId");

            migrationBuilder.CreateIndex(
                name: "IX_NHSActivities_NHSActivityId",
                table: "NHSActivities",
                column: "NHSActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationId",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PathwayStatuses_PathwayStatusId",
                table: "PathwayStatuses",
                column: "PathwayStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserAccountUserId",
                table: "RefreshToken",
                column: "UserAccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RTTs_RTTId",
                table: "RTTs",
                column: "RTTId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_NHS_DiagnosticDiagnosticId",
                table: "Specialties",
                column: "NHS_DiagnosticDiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_NHS_ReferralReferralId",
                table: "Specialties",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_SpecialtyId",
                table: "Specialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_States_StateId",
                table: "States",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_StructureDefinitions_StructureDefinitionId",
                table: "StructureDefinitions",
                column: "StructureDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_CompanyStructureId",
                table: "UserAccesses",
                column: "CompanyStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_UserAccessId",
                table: "UserAccesses",
                column: "UserAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_UserAccountId",
                table: "UserAccesses",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountAdditionalActivities_UserAccountAdditionalActivi~",
                table: "UserAccountAdditionalActivities",
                column: "UserAccountAdditionalActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountAdditionalActivities_UserAccountId",
                table: "UserAccountAdditionalActivities",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountAdditionalActivities_UserActivityId",
                table: "UserAccountAdditionalActivities",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountRoles_UserAccountId",
                table: "UserAccountRoles",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountRoles_UserAccountRoleId",
                table: "UserAccountRoles",
                column: "UserAccountRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountRoles_UserRoleDefinitionId",
                table: "UserAccountRoles",
                column: "UserRoleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserId",
                table: "UserAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_FirstName_LastName",
                table: "UserAccounts",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountSettings_UserAccountSettingsId",
                table: "UserAccountSettings",
                column: "UserAccountSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserActivityId",
                table: "UserActivities",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserActivityParentId",
                table: "UserActivities",
                column: "UserActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityParents_UserActivityParentId",
                table: "UserActivityParents",
                column: "UserActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserActivityId",
                table: "UserRoleActivities",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserRoleActivityId",
                table: "UserRoleActivities",
                column: "UserRoleActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserRoleDefinitionId",
                table: "UserRoleActivities",
                column: "UserRoleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDefinitions_UserRoleDefinitionId",
                table: "UserRoleDefinitions",
                column: "UserRoleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingTypes_WaitingTypeId",
                table: "WaitingTypes",
                column: "WaitingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_WardId",
                table: "Wards",
                column: "WardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTypes");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "Consultants");

            migrationBuilder.DropTable(
                name: "CustomIdentityFormatSettings");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "GeneralInformations");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "NHS_Appointments");

            migrationBuilder.DropTable(
                name: "NHS_DiagnosticResults");

            migrationBuilder.DropTable(
                name: "NHS_Patient_Validation_Details");

            migrationBuilder.DropTable(
                name: "NHS_Patientdocuments");

            migrationBuilder.DropTable(
                name: "NHS_Waitinglists");

            migrationBuilder.DropTable(
                name: "NHSActivities");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PathwayStatuses");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RTTs");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "UserAccesses");

            migrationBuilder.DropTable(
                name: "UserAccountAdditionalActivities");

            migrationBuilder.DropTable(
                name: "UserAccountRoles");

            migrationBuilder.DropTable(
                name: "UserAccountSettings");

            migrationBuilder.DropTable(
                name: "UserRoleActivities");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "NHS_Patients");

            migrationBuilder.DropTable(
                name: "NHS_Patient_Validations");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "WaitingTypes");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "CompanyStructures");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserRoleDefinitions");

            migrationBuilder.DropTable(
                name: "NHS_Diagnostics");

            migrationBuilder.DropTable(
                name: "NHS_Referrals");

            migrationBuilder.DropTable(
                name: "StructureDefinitions");

            migrationBuilder.DropTable(
                name: "UserActivityParents");
        }
    }
}
