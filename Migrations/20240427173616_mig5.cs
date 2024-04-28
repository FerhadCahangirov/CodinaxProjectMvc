using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advice_Courses_FirstAdvicedCourseId",
                table: "Advice");

            migrationBuilder.DropForeignKey(
                name: "FK_Advice_Courses_SecondAdvicedCourseId",
                table: "Advice");

            migrationBuilder.DropForeignKey(
                name: "FK_Advice_Templates_TemplateId",
                table: "Advice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advice",
                table: "Advice");

            migrationBuilder.RenameTable(
                name: "Advice",
                newName: "Advices");

            migrationBuilder.RenameIndex(
                name: "IX_Advice_TemplateId",
                table: "Advices",
                newName: "IX_Advices_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Advice_SecondAdvicedCourseId",
                table: "Advices",
                newName: "IX_Advices_SecondAdvicedCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Advice_FirstAdvicedCourseId",
                table: "Advices",
                newName: "IX_Advices_FirstAdvicedCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advices",
                table: "Advices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advices_Courses_FirstAdvicedCourseId",
                table: "Advices",
                column: "FirstAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advices_Courses_SecondAdvicedCourseId",
                table: "Advices",
                column: "SecondAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advices_Templates_TemplateId",
                table: "Advices",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advices_Courses_FirstAdvicedCourseId",
                table: "Advices");

            migrationBuilder.DropForeignKey(
                name: "FK_Advices_Courses_SecondAdvicedCourseId",
                table: "Advices");

            migrationBuilder.DropForeignKey(
                name: "FK_Advices_Templates_TemplateId",
                table: "Advices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advices",
                table: "Advices");

            migrationBuilder.RenameTable(
                name: "Advices",
                newName: "Advice");

            migrationBuilder.RenameIndex(
                name: "IX_Advices_TemplateId",
                table: "Advice",
                newName: "IX_Advice_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Advices_SecondAdvicedCourseId",
                table: "Advice",
                newName: "IX_Advice_SecondAdvicedCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Advices_FirstAdvicedCourseId",
                table: "Advice",
                newName: "IX_Advice_FirstAdvicedCourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advice",
                table: "Advice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advice_Courses_FirstAdvicedCourseId",
                table: "Advice",
                column: "FirstAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advice_Courses_SecondAdvicedCourseId",
                table: "Advice",
                column: "SecondAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advice_Templates_TemplateId",
                table: "Advice",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
