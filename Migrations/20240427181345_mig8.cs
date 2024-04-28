using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_SecondAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SecondAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SecondAdvicedCourseId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FirstAdvicedCourseId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecondAdvicedCourseId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FirstAdvicedCourseId",
                table: "Courses",
                column: "FirstAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SecondAdvicedCourseId",
                table: "Courses",
                column: "SecondAdvicedCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_FirstAdvicedCourseId",
                table: "Courses",
                column: "FirstAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_SecondAdvicedCourseId",
                table: "Courses",
                column: "SecondAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
