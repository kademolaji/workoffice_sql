using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkOffice.Domain.Migrations
{
    public partial class nhs_referral_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NHS_DiagnosticDiagnosticId",
                table: "Specialties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHS_ReferralReferralId",
                table: "Specialties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHS_ReferralReferralId",
                table: "NHS_Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NHS_ReferralReferralId",
                table: "Consultants",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_NHS_DiagnosticDiagnosticId",
                table: "Specialties",
                column: "NHS_DiagnosticDiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_NHS_ReferralReferralId",
                table: "Specialties",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients",
                column: "NHS_DiagnosticDiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_NHS_ReferralReferralId",
                table: "NHS_Patients",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_NHS_ReferralReferralId",
                table: "Consultants",
                column: "NHS_ReferralReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_DiagnosticResults_DiagnosticResultId",
                table: "NHS_DiagnosticResults",
                column: "DiagnosticResultId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Diagnostics_DiagnosticId",
                table: "NHS_Diagnostics",
                column: "DiagnosticId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Referrals_ReferralId",
                table: "NHS_Referrals",
                column: "ReferralId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_NHS_Referrals_NHS_ReferralReferralId",
                table: "Consultants",
                column: "NHS_ReferralReferralId",
                principalTable: "NHS_Referrals",
                principalColumn: "ReferralId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NHS_Patients_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients",
                column: "NHS_DiagnosticDiagnosticId",
                principalTable: "NHS_Diagnostics",
                principalColumn: "DiagnosticId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NHS_Patients_NHS_Referrals_NHS_ReferralReferralId",
                table: "NHS_Patients",
                column: "NHS_ReferralReferralId",
                principalTable: "NHS_Referrals",
                principalColumn: "ReferralId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                table: "Specialties",
                column: "NHS_DiagnosticDiagnosticId",
                principalTable: "NHS_Diagnostics",
                principalColumn: "DiagnosticId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_NHS_Referrals_NHS_ReferralReferralId",
                table: "Specialties",
                column: "NHS_ReferralReferralId",
                principalTable: "NHS_Referrals",
                principalColumn: "ReferralId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_NHS_Referrals_NHS_ReferralReferralId",
                table: "Consultants");

            migrationBuilder.DropForeignKey(
                name: "FK_NHS_Patients_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_NHS_Patients_NHS_Referrals_NHS_ReferralReferralId",
                table: "NHS_Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_NHS_Diagnostics_NHS_DiagnosticDiagnosticId",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_NHS_Referrals_NHS_ReferralReferralId",
                table: "Specialties");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "NHS_DiagnosticResults");

            migrationBuilder.DropTable(
                name: "NHS_Diagnostics");

            migrationBuilder.DropTable(
                name: "NHS_Referrals");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_NHS_DiagnosticDiagnosticId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_NHS_ReferralReferralId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_NHS_Patients_NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients");

            migrationBuilder.DropIndex(
                name: "IX_NHS_Patients_NHS_ReferralReferralId",
                table: "NHS_Patients");

            migrationBuilder.DropIndex(
                name: "IX_Consultants_NHS_ReferralReferralId",
                table: "Consultants");

            migrationBuilder.DropColumn(
                name: "NHS_DiagnosticDiagnosticId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "NHS_ReferralReferralId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "NHS_DiagnosticDiagnosticId",
                table: "NHS_Patients");

            migrationBuilder.DropColumn(
                name: "NHS_ReferralReferralId",
                table: "NHS_Patients");

            migrationBuilder.DropColumn(
                name: "NHS_ReferralReferralId",
                table: "Consultants");
        }
    }
}
