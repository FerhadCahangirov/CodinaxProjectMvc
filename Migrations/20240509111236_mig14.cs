using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureFile_Lectures_LectureId",
                table: "LectureFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_LectureVideos_LectureVideoId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Modules_SectionId",
                table: "Lectures");

            migrationBuilder.DropTable(
                name: "LectureVideos");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_LectureVideoId",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LectureFile",
                table: "LectureFile");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LectureNumber",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LectureVideoId",
                table: "Lectures");

            migrationBuilder.RenameTable(
                name: "LectureFile",
                newName: "LectureFiles");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Lectures",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_SectionId",
                table: "Lectures",
                newName: "IX_Lectures_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_LectureFile_LectureId",
                table: "LectureFiles",
                newName: "IX_LectureFiles_LectureId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LectureFiles",
                table: "LectureFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureFiles_Lectures_LectureId",
                table: "LectureFiles",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Modules_ModuleId",
                table: "Lectures",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LectureFiles_Lectures_LectureId",
                table: "LectureFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Modules_ModuleId",
                table: "Lectures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LectureFiles",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LectureFiles");

            migrationBuilder.RenameTable(
                name: "LectureFiles",
                newName: "LectureFile");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Lectures",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_ModuleId",
                table: "Lectures",
                newName: "IX_Lectures_SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_LectureFiles_LectureId",
                table: "LectureFile",
                newName: "IX_LectureFile_LectureId");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Lectures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LectureNumber",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LectureVideoId",
                table: "Lectures",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LectureFile",
                table: "LectureFile",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LectureVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PathOrContainer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureVideos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_LectureVideoId",
                table: "Lectures",
                column: "LectureVideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LectureFile_Lectures_LectureId",
                table: "LectureFile",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_LectureVideos_LectureVideoId",
                table: "Lectures",
                column: "LectureVideoId",
                principalTable: "LectureVideos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Modules_SectionId",
                table: "Lectures",
                column: "SectionId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
