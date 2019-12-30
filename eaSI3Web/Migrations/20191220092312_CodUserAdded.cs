using Microsoft.EntityFrameworkCore.Migrations;

namespace eaSI3Web.Migrations
{
    public partial class CodUserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodUser",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodUser",
                table: "Users");
        }
    }
}
