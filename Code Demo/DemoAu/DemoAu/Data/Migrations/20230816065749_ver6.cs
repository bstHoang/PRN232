using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoAu.Data.Migrations
{
    public partial class ver6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewProperty",
                table: "AspNetUsers",
                newName: "Major");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Major",
                table: "AspNetUsers",
                newName: "NewProperty");
        }
    }
}
