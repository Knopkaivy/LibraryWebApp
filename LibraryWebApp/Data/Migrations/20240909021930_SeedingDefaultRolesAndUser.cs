using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0428efdd-bbf9-44fa-bba3-0a1873af0e56", null, "Librarian", "LIBRARIAN" },
                    { "b117d060-6194-4e16-8c49-f60bbf42ec3e", null, "Administrator", "ADMINISTRATOR" },
                    { "fdf41afb-4841-43b9-8642-b32179934a49", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf", 0, "02e70e08-1638-4a1f-b14a-56384372ff51", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAIAAYagAAAAEEjAtFZIiEOhTNnL1jy0aDsdP5MR7Up8OR4AQmVn8RX7sgpAhXtKmtv2bZw7LhcoIw==", null, false, "1048279a-a78a-479f-b8c8-eeb6eb1d23b9", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b117d060-6194-4e16-8c49-f60bbf42ec3e", "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0428efdd-bbf9-44fa-bba3-0a1873af0e56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fdf41afb-4841-43b9-8642-b32179934a49");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b117d060-6194-4e16-8c49-f60bbf42ec3e", "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b117d060-6194-4e16-8c49-f60bbf42ec3e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf");
        }
    }
}
