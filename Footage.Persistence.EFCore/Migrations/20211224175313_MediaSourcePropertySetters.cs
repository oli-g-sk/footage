using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class MediaSourcePropertySetters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeSubfolders",
                table: "MediaSources",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MediaSources",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RootPath",
                table: "MediaSources",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeSubfolders",
                table: "MediaSources");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MediaSources");

            migrationBuilder.DropColumn(
                name: "RootPath",
                table: "MediaSources");
        }
    }
}
