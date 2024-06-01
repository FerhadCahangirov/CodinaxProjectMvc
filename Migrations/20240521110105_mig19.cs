using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_LectureFile_LectureFileId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_LectureFile_Lectures_LectureId",
                table: "LectureFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LectureFile",
                table: "LectureFile");

            migrationBuilder.RenameTable(
                name: "LectureFile",
                newName: "LectureFiles");

            migrationBuilder.RenameIndex(
                name: "IX_LectureFile_LectureId",
                table: "LectureFiles",
                newName: "IX_LectureFiles_LectureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LectureFiles",
                table: "LectureFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_LectureFiles_LectureFileId",
                table: "Bookmarks",
                column: "LectureFileId",
                principalTable: "LectureFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LectureFiles_Lectures_LectureId",
                table: "LectureFiles",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_LectureFiles_LectureFileId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_LectureFiles_Lectures_LectureId",
                table: "LectureFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LectureFiles",
                table: "LectureFiles");

            migrationBuilder.RenameTable(
                name: "LectureFiles",
                newName: "LectureFile");

            migrationBuilder.RenameIndex(
                name: "IX_LectureFiles_LectureId",
                table: "LectureFile",
                newName: "IX_LectureFile_LectureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LectureFile",
                table: "LectureFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_LectureFile_LectureFileId",
                table: "Bookmarks",
                column: "LectureFileId",
                principalTable: "LectureFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LectureFile_Lectures_LectureId",
                table: "LectureFile",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
