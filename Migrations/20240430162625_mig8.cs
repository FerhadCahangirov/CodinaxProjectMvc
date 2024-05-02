using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Subscribers",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "Subscribers",
                newName: "IsEmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                table: "Subscribers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                table: "Subscribers",
                newName: "IsArchived");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Subscribers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Subscribers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Subscribers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "IsEmailConfirmed",
                table: "Subscribers",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Subscribers",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "Subscribers",
                newName: "LockoutEnabled");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Subscribers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Subscribers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Subscribers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
