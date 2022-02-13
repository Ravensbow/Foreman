using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foreman.Server.Migrations
{
    public partial class ForemanFileIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Courses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ContentHash",
                table: "Files",
                column: "ContentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ContextId_Component_ItemId",
                table: "Files",
                columns: new[] { "ContextId", "Component", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_PathNameHash",
                table: "Files",
                column: "PathNameHash",
                unique: true,
                filter: "[PathNameHash] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_ContentHash",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ContextId_Component_ItemId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_PathNameHash",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "Courses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Courses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
