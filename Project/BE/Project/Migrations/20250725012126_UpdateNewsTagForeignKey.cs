using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewsTagForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTags_News_NewsId1",
                table: "NewsTags");

            migrationBuilder.RenameColumn(
                name: "NewsId1",
                table: "NewsTags",
                newName: "NewsId");

            migrationBuilder.RenameIndex(
                name: "IX_NewsTags_NewsId1",
                table: "NewsTags",
                newName: "IX_NewsTags_NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTags_News_NewsId",
                table: "NewsTags",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTags_News_NewsId",
                table: "NewsTags");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "NewsTags",
                newName: "NewsId1");

            migrationBuilder.RenameIndex(
                name: "IX_NewsTags_NewsId",
                table: "NewsTags",
                newName: "IX_NewsTags_NewsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTags_News_NewsId1",
                table: "NewsTags",
                column: "NewsId1",
                principalTable: "News",
                principalColumn: "Id");
        }
    }
}
