using Microsoft.EntityFrameworkCore.Migrations;

namespace PekoBot.Database.Migrations
{
    public partial class VTuber_Platform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "VTubers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Platform",
                table: "VTubers");
        }
    }
}
