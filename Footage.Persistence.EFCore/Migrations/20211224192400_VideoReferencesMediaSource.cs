using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class VideoReferencesMediaSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaSourceId",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaSourceId",
                table: "Videos");
        }
    }
}
