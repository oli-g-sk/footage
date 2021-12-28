using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class BookmarkTimeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Time",
                table: "Bookmarks",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Bookmarks");
        }
    }
}
