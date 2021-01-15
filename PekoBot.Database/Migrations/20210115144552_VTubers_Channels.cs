using Microsoft.EntityFrameworkCore.Migrations;

namespace PekoBot.Database.Migrations
{
    public partial class VTubers_Channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VTuberId",
                table: "Channels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_VTuberId",
                table: "Channels",
                column: "VTuberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_VTubers_VTuberId",
                table: "Channels",
                column: "VTuberId",
                principalTable: "VTubers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_VTubers_VTuberId",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_VTuberId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "VTuberId",
                table: "Channels");
        }
    }
}
