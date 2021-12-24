using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class MediaSourceNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "MediaSources",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "MediaSources",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "MediaSources");

            migrationBuilder.DropColumn(
                name: "Available",
                table: "MediaSources");
        }
    }
}
