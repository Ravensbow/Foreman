using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foreman.Server.Migrations
{
    public partial class FileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Institution_InstitutionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCategories_Institution_InstitutionId",
                table: "CourseCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Institution_InstitutionId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Institution_AspNetUsers_OwnerId",
                table: "Institution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institution",
                table: "Institution");

            migrationBuilder.RenameTable(
                name: "Institution",
                newName: "Institutions");

            migrationBuilder.RenameIndex(
                name: "IX_Institution_OwnerId",
                table: "Institutions",
                newName: "IX_Institutions_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentHash = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    PathNameHash = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    ContextId = table.Column<int>(type: "int", nullable: false),
                    Component = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MimeType = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Institutions_InstitutionId",
                table: "AspNetUsers",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCategories_Institutions_InstitutionId",
                table: "CourseCategories",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Institutions_InstitutionId",
                table: "Courses",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Institutions_AspNetUsers_OwnerId",
                table: "Institutions",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Institutions_InstitutionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCategories_Institutions_InstitutionId",
                table: "CourseCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Institutions_InstitutionId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Institutions_AspNetUsers_OwnerId",
                table: "Institutions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions");

            migrationBuilder.RenameTable(
                name: "Institutions",
                newName: "Institution");

            migrationBuilder.RenameIndex(
                name: "IX_Institutions_OwnerId",
                table: "Institution",
                newName: "IX_Institution_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institution",
                table: "Institution",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Institution_InstitutionId",
                table: "AspNetUsers",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCategories_Institution_InstitutionId",
                table: "CourseCategories",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Institution_InstitutionId",
                table: "Courses",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_AspNetUsers_OwnerId",
                table: "Institution",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
