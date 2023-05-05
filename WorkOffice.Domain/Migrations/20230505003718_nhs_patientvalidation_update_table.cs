using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace WorkOffice.Domain.Migrations
{
    public partial class nhs_patientvalidationdetails_update_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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
                    SpecialtyId = table.Column<int>(nullable: true),
                    RTTId = table.Column<int>(maxLength: 50, nullable: true),
                    RTTWait = table.Column<string>(maxLength: 550, nullable: true),
                    DistrictNumber = table.Column<string>(maxLength: 50, nullable: true),
                    NHSNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    PathwayStatusId = table.Column<long>(nullable: true),
                    //SpecialtyId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patient_Validations", x => x.PatientValidationId);
                    table.ForeignKey(
                        name: "FK_NHS_Patient_Validations_PathwayStatuses_PathwayStatusId",
                        column: x => x.PathwayStatusId,
                        principalTable: "PathwayStatuses",
                        principalColumn: "PathwayStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Patient_Validations_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "SpecialtyId",
                        onDelete: ReferentialAction.Restrict);
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
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    NHS_PatientPatientId = table.Column<int>(nullable: true),
                    NHS_Patient_ValidationPatientValidationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patient_Validation_Details", x => x.PatientValidationDetailsId);
                    table.ForeignKey(
                        name: "FK_NHS_Patient_Validation_Details_NHS_Patients_NHS_PatientPati~",
                        column: x => x.NHS_PatientPatientId,
                        principalTable: "NHS_Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Patient_Validation_Details_NHS_Patient_Validations_NHS_~",
                        column: x => x.NHS_Patient_ValidationPatientValidationId,
                        principalTable: "NHS_Patient_Validations",
                        principalColumn: "PatientValidationId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

       
    }
}
