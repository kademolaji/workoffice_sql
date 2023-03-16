using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkOffice.Domain.Migrations
{
    public partial class addedindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StructureDefinitions_Definition",
                table: "StructureDefinitions");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDefinitions_UserRoleDefinitionId",
                table: "UserRoleDefinitions",
                column: "UserRoleDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleActivities_UserRoleActivityId",
                table: "UserRoleActivities",
                column: "UserRoleActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityParents_UserActivityParentId",
                table: "UserActivityParents",
                column: "UserActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserActivityId",
                table: "UserActivities",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountSettings_UserAccountSettingsId",
                table: "UserAccountSettings",
                column: "UserAccountSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountAdditionalActivities_UserAccountAdditionalActivi~",
                table: "UserAccountAdditionalActivities",
                column: "UserAccountAdditionalActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccesses_UserAccessId",
                table: "UserAccesses",
                column: "UserAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_StructureDefinitions_StructureDefinitionId",
                table: "StructureDefinitions",
                column: "StructureDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_States_StateId",
                table: "States",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationId",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationId",
                table: "Locations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInformations_GeneralInformationId",
                table: "GeneralInformations",
                column: "GeneralInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomIdentityFormatSettings_CustomIdentityFormatSettingId",
                table: "CustomIdentityFormatSettings",
                column: "CustomIdentityFormatSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryId",
                table: "Countries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_CompanyStructureId",
                table: "CompanyStructures",
                column: "CompanyStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_AuditTrailId",
                table: "AuditTrails",
                column: "AuditTrailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoleDefinitions_UserRoleDefinitionId",
                table: "UserRoleDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleActivities_UserRoleActivityId",
                table: "UserRoleActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivityParents_UserActivityParentId",
                table: "UserActivityParents");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_UserActivityId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountSettings_UserAccountSettingsId",
                table: "UserAccountSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountAdditionalActivities_UserAccountAdditionalActivi~",
                table: "UserAccountAdditionalActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserAccesses_UserAccessId",
                table: "UserAccesses");

            migrationBuilder.DropIndex(
                name: "IX_StructureDefinitions_StructureDefinitionId",
                table: "StructureDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_States_StateId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Locations_LocationId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_GeneralInformations_GeneralInformationId",
                table: "GeneralInformations");

            migrationBuilder.DropIndex(
                name: "IX_CustomIdentityFormatSettings_CustomIdentityFormatSettingId",
                table: "CustomIdentityFormatSettings");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_CompanyStructures_CompanyStructureId",
                table: "CompanyStructures");

            migrationBuilder.DropIndex(
                name: "IX_AuditTrails_AuditTrailId",
                table: "AuditTrails");

            migrationBuilder.CreateIndex(
                name: "IX_StructureDefinitions_Definition",
                table: "StructureDefinitions",
                column: "Definition",
                unique: true);
        }
    }
}
