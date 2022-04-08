using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Footage.Migrations
{
    public partial class RemovedThumbnailEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoThumbnail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoThumbnail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoThumbnail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoThumbnail_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoThumbnail_VideoId",
                table: "VideoThumbnail",
                column: "VideoId",
                unique: true);
        }
    }
}
