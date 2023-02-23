using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkOffice.Domain.Migrations
{
    public partial class NHSbackendupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "AppTypes",
                columns: table => new
                {
                    AppTypeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypes", x => x.AppTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Consultants",
                columns: table => new
                {
                    ConsultantId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultants", x => x.ConsultantId);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalId);
                });

            migrationBuilder.CreateTable(
                name: "PathwayStatuses",
                columns: table => new
                {
                    PathwayStatusId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
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
                    RTTId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RTTs", x => x.RTTId);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.SpecialtyId);
                });

            migrationBuilder.CreateTable(
                name: "WaitingTypes",
                columns: table => new
                {
                    WaitingTypeId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
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
                    WardId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityId",
                table: "Activities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTypes_AppTypeId",
                table: "AppTypes",
                column: "AppTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_ConsultantId",
                table: "Consultants",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_HospitalId",
                table: "Hospitals",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_PathwayStatuses_PathwayStatusId",
                table: "PathwayStatuses",
                column: "PathwayStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RTTs_RTTId",
                table: "RTTs",
                column: "RTTId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_SpecialtyId",
                table: "Specialties",
                column: "SpecialtyId");

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
                name: "Activities");

            migrationBuilder.DropTable(
                name: "AppTypes");

            migrationBuilder.DropTable(
                name: "Consultants");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "PathwayStatuses");

            migrationBuilder.DropTable(
                name: "RTTs");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "WaitingTypes");

            migrationBuilder.DropTable(
                name: "Wards");
        }
    }
}
