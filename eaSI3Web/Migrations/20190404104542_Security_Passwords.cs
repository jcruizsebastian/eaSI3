using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eaSI3Web.Migrations
{
    public partial class Security_Passwords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SI3Password",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSi3_Encrypted",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_Encrypted",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSi3_Encrypted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password_Encrypted",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "JiraPassword",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SI3Password",
                table: "Users",
                nullable: true);
        }
    }
}
