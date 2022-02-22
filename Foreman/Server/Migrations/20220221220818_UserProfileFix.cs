using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foreman.Server.Migrations
{
    public partial class UserProfileFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnedInstitutionId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnedInstitutionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
