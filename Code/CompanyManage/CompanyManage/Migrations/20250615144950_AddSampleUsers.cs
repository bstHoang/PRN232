using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyManage.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DepartmentId", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PositionId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "bc558690-ba79-410f-9db5-026ced20f23e", 2, "admin@company.com", true, false, null, "Admin User", "ADMIN@COMPANY.COM", "ADMIN", "AQAAAAIAAYagAAAAEMj++vFtn3GY5t/8ZrFDuCARd2T4I3oQBfxF6iphfu2hX9PT9uiPAHXAOa650EkqRw==", null, false, 2, null, false, "admin" },
                    { 2, 0, "922f9f18-6101-46d2-b1d4-6eb65fe707e2", 1, "hrmanager@company.com", true, false, null, "HR Manager", "HRMANAGER@COMPANY.COM", "HRMANAGER", "AQAAAAIAAYagAAAAENVIwxs7P5iQ6OvR8QyTVHucz/NzQcpR1PHQTNzzTdeEh5sqlr9rC/9KwA1gGrL9RA==", null, false, 1, null, false, "hrmanager" },
                    { 3, 0, "541e8f6d-8ade-46ae-95bb-f6642d29725e", 1, "hremployee@company.com", true, false, null, "HR Employee", "HREMPLOYEE@COMPANY.COM", "HREMPLOYEE", "AQAAAAIAAYagAAAAENFGKKpVfAXRppQnm51AgiiF2VWt37emQCRu0f5vFA38XJMcvXjtciNeXNOtiYw8MQ==", null, false, 3, null, false, "hremployee" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 2, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
