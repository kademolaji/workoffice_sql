using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkOffice.Domain.Migrations
{
    public partial class AdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    AuditTrailId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    CountryId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    CustomIdentityFormatSettingId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Prefix = table.Column<int>(nullable: false),
                    Suffix = table.Column<int>(nullable: false),
                    Digits = table.Column<int>(nullable: false),
                    Company = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomIdentityFormatSettings", x => x.CustomIdentityFormatSettingId);
                });

            migrationBuilder.CreateTable(
                name: "GeneralInformations",
                columns: table => new
                {
                    GeneralInformationId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "StructureDefinitions",
                columns: table => new
                {
                    StructureDefinitionId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    RoleId = table.Column<int>(nullable: false),
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
                    UserAccountSettingsId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    UserActivityParentId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
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
                    UserRoleDefinitionId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleDefinitions", x => x.UserRoleDefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CountryId = table.Column<Guid>(nullable: false)
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
                name: "CompanyStructures",
                columns: table => new
                {
                    CompanyStructureId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    StructureTypeID = table.Column<Guid>(maxLength: 50, nullable: false),
                    Country = table.Column<string>(maxLength: 250, nullable: true),
                    Parent = table.Column<string>(maxLength: 250, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    CompanyHead = table.Column<string>(maxLength: 100, nullable: true),
                    ParentID = table.Column<Guid>(nullable: true),
                    Company = table.Column<string>(maxLength: 250, nullable: true),
                    StructureDefinitionId = table.Column<Guid>(nullable: true)
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
                    RefreshTokenId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountUserId = table.Column<Guid>(nullable: false),
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
                    UserActivityId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserActivityName = table.Column<string>(maxLength: 256, nullable: false),
                    UserActivityParentId = table.Column<Guid>(nullable: false)
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
                    UserAccountRoleId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountId = table.Column<Guid>(nullable: false),
                    UserRoleDefinitionId = table.Column<Guid>(nullable: false)
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
                name: "UserAccesses",
                columns: table => new
                {
                    UserAccessId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CompanyStructureId = table.Column<Guid>(nullable: false),
                    UserAccountId = table.Column<Guid>(nullable: false)
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
                    UserAccountAdditionalActivityId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserAccountId = table.Column<Guid>(nullable: false),
                    UserActivityId = table.Column<Guid>(nullable: false),
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
                    UserRoleActivityId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserRoleDefinitionId = table.Column<Guid>(nullable: false),
                    UserActivityId = table.Column<Guid>(nullable: false),
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
                name: "IX_CompanyStructures_StructureDefinitionId",
                table: "CompanyStructures",
                column: "StructureDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserAccountUserId",
                table: "RefreshToken",
                column: "UserAccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_StructureDefinitions_Definition",
                table: "StructureDefinitions",
                column: "Definition",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_CompanyStructureId",
                table: "UserAccesses",
                column: "CompanyStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_UserAccountId",
                table: "UserAccesses",
                column: "UserAccountId");

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
                name: "IX_UserAccountRoles_UserRoleDefinitionId",
                table: "UserAccountRoles",
                column: "UserRoleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserActivityParentId",
                table: "UserActivities",
                column: "UserActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserActivityId",
                table: "UserRoleActivities",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserRoleDefinitionId",
                table: "UserRoleActivities",
                column: "UserRoleDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "CustomIdentityFormatSettings");

            migrationBuilder.DropTable(
                name: "GeneralInformations");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RefreshToken");

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
                name: "StructureDefinitions");

            migrationBuilder.DropTable(
                name: "UserActivityParents");
        }
    }
}
