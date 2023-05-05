using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WorkOffice.Domain.Migrations
{
    public partial class nhs_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Activity",
                table: "CustomIdentityFormatSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Separator",
                table: "CustomIdentityFormatSettings",
                nullable: true);

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
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Patients", x => x.PatientId);
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
                    PatientId = table.Column<int>(nullable: true),
                    patientValidationId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    AppointmentStatus = table.Column<string>(maxLength: 50, nullable: true),
                    CancellationReason = table.Column<string>(maxLength: 50, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    AppTypeId1 = table.Column<long>(nullable: true),
                    ConsultantId1 = table.Column<long>(nullable: true),
                    HospitalId1 = table.Column<long>(nullable: true),
                    NHS_PatientPatientId = table.Column<int>(nullable: true),
                    NHS_Patient_ValidationPatientValidationId = table.Column<int>(nullable: true),
                    SpecialtyId = table.Column<long>(nullable: true),
                    WardId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHS_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_AppTypes_AppTypeId1",
                        column: x => x.AppTypeId1,
                        principalTable: "AppTypes",
                        principalColumn: "AppTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_Consultants_ConsultantId1",
                        column: x => x.ConsultantId1,
                        principalTable: "Consultants",
                        principalColumn: "ConsultantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_Hospitals_HospitalId1",
                        column: x => x.HospitalId1,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_NHS_Patients_NHS_PatientPatientId",
                        column: x => x.NHS_PatientPatientId,
                        principalTable: "NHS_Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_NHS_Patient_Validations_NHS_Patient_Valida~",
                        column: x => x.NHS_Patient_ValidationPatientValidationId,
                        principalTable: "NHS_Patient_Validations",
                        principalColumn: "PatientValidationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "SpecialtyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NHS_Appointments_Wards_WardId1",
                        column: x => x.WardId1,
                        principalTable: "Wards",
                        principalColumn: "WardId",
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

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_AppTypeId1",
                table: "NHS_Appointments",
                column: "AppTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_AppointmentId",
                table: "NHS_Appointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_ConsultantId1",
                table: "NHS_Appointments",
                column: "ConsultantId1");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_HospitalId1",
                table: "NHS_Appointments",
                column: "HospitalId1");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_NHS_PatientPatientId",
                table: "NHS_Appointments",
                column: "NHS_PatientPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_NHS_Patient_ValidationPatientValidationId",
                table: "NHS_Appointments",
                column: "NHS_Patient_ValidationPatientValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_SpecialtyId",
                table: "NHS_Appointments",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Appointments_WardId1",
                table: "NHS_Appointments",
                column: "WardId1");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validation_Details_NHS_PatientPatientId",
                table: "NHS_Patient_Validation_Details",
                column: "NHS_PatientPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validation_Details_NHS_Patient_ValidationPatien~",
                table: "NHS_Patient_Validation_Details",
                column: "NHS_Patient_ValidationPatientValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validation_Details_PatientValidationDetailsId",
                table: "NHS_Patient_Validation_Details",
                column: "PatientValidationDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validations_PathwayStatusId",
                table: "NHS_Patient_Validations",
                column: "PathwayStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validations_PatientValidationId",
                table: "NHS_Patient_Validations",
                column: "PatientValidationId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patient_Validations_SpecialtyId",
                table: "NHS_Patient_Validations",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patientdocuments_PatientDocumentId",
                table: "NHS_Patientdocuments",
                column: "PatientDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_NHS_Patients_PatientId",
                table: "NHS_Patients",
                column: "PatientId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NHS_Appointments");

            migrationBuilder.DropTable(
                name: "NHS_Patient_Validation_Details");

            migrationBuilder.DropTable(
                name: "NHS_Patientdocuments");

            migrationBuilder.DropTable(
                name: "NHS_Waitinglists");

            migrationBuilder.DropTable(
                name: "NHS_Patients");

            migrationBuilder.DropTable(
                name: "NHS_Patient_Validations");

            migrationBuilder.DropColumn(
                name: "Activity",
                table: "CustomIdentityFormatSettings");

            migrationBuilder.DropColumn(
                name: "Separator",
                table: "CustomIdentityFormatSettings");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
