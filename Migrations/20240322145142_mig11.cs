using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_InstructorId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_InstructorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InstructorId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_InstructorId",
                table: "Categories",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_InstructorId",
                table: "Categories",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
