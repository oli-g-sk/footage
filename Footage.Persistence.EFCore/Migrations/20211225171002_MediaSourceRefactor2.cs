using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class MediaSourceRefactor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Videos_MediaSourceId",
                table: "Videos",
                column: "MediaSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_MediaSources_MediaSourceId",
                table: "Videos",
                column: "MediaSourceId",
                principalTable: "MediaSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_MediaSources_MediaSourceId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_MediaSourceId",
                table: "Videos");
        }
    }
}
