using Microsoft.EntityFrameworkCore.Migrations;

namespace eaSI3Web.Migrations
{
    public partial class Submit_Work : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Submit",
                table: "WorkTracking",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Submit",
                table: "WorkTracking");
        }
    }
}
