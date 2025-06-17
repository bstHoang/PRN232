using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyManage.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityStampToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c23f48c-4b07-4293-b4ee-f906fcfa7f8a", "AQAAAAIAAYagAAAAENCfoz2892Gl7HweieNC9bM+vY5hc2A/QobCniK5BJBHWfDiKSnVyszxcE+IDtuPVA==", "06baf3d8-96d8-485a-bc28-ebab39e6b4c8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02afbf6e-3297-4805-8178-f7f1af74f370", "AQAAAAIAAYagAAAAEG2jx2jTiVUaBu+aKok6lzSg4vagPbNNsJWs2/5Kw0hjGxBEW0aW6G3BcDq9OsrCWA==", "de30bcf9-968f-4bb7-a00a-264731696e63" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a360be10-45b2-4141-86fc-534f80a5c4c4", "AQAAAAIAAYagAAAAEJRfXeFaHTLi4xQQXxXG2HNgifzPZN5kFWg1M7zGyAoB0EYOwzKcizYlUI5Ha9BMog==", "1c38e5ef-ef2b-441f-ad86-698e628bd490" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bc558690-ba79-410f-9db5-026ced20f23e", "AQAAAAIAAYagAAAAEMj++vFtn3GY5t/8ZrFDuCARd2T4I3oQBfxF6iphfu2hX9PT9uiPAHXAOa650EkqRw==", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "922f9f18-6101-46d2-b1d4-6eb65fe707e2", "AQAAAAIAAYagAAAAENVIwxs7P5iQ6OvR8QyTVHucz/NzQcpR1PHQTNzzTdeEh5sqlr9rC/9KwA1gGrL9RA==", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "541e8f6d-8ade-46ae-95bb-f6642d29725e", "AQAAAAIAAYagAAAAENFGKKpVfAXRppQnm51AgiiF2VWt37emQCRu0f5vFA38XJMcvXjtciNeXNOtiYw8MQ==", null });
        }
    }
}
