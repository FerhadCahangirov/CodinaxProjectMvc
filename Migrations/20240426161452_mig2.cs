using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Price_Courses_CourseId",
                table: "Price");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Courses_CourseId",
                table: "Tools");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_CourseId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Tools_CourseId",
                table: "Tools");

            migrationBuilder.DropIndex(
                name: "IX_Price_CourseId",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Price");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Tools",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Price",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Topics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Tools",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Tools",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateId",
                table: "Price",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Price",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CourseId",
                table: "Topics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Tools_CourseId",
                table: "Tools",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_CourseId",
                table: "Price",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Price_Courses_CourseId",
                table: "Price",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Courses_CourseId",
                table: "Tools",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
