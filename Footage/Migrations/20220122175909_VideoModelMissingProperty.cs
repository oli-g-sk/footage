using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class VideoModelMissingProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMissing",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMissing",
                table: "Videos");
        }
    }
}
