using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkOffice.Domain.Migrations
{
    public partial class lastdigittocustomstettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastDigit",
                table: "CustomIdentityFormatSettings",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDigit",
                table: "CustomIdentityFormatSettings");
        }
    }
}
