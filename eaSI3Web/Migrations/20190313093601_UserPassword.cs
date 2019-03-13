using Microsoft.EntityFrameworkCore.Migrations;

namespace eaSI3Web.Migrations
{
    public partial class UserPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JiraPassword",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SI3Password",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SI3Password",
                table: "Users");
        }
    }
}
