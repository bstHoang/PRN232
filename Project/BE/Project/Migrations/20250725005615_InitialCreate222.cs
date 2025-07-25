using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsId1",
                table: "NewsTags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsTags_NewsId1",
                table: "NewsTags",
                column: "NewsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTags_News_NewsId1",
                table: "NewsTags",
                column: "NewsId1",
                principalTable: "News",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTags_News_NewsId1",
                table: "NewsTags");

            migrationBuilder.DropIndex(
                name: "IX_NewsTags_NewsId1",
                table: "NewsTags");

            migrationBuilder.DropColumn(
                name: "NewsId1",
                table: "NewsTags");
        }
    }
}
