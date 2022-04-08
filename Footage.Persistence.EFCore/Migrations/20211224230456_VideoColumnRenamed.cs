using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class VideoColumnRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Videos",
                newName: "MediaSourceUri");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaSourceUri",
                table: "Videos",
                newName: "Uri");
        }
    }
}
