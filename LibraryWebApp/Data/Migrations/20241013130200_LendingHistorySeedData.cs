using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class LendingHistorySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c2a4bda-2abb-4ddd-865e-68fb98d337a3", "AQAAAAIAAYagAAAAEJI94pUuOCR0wKo3kHCCnOevzo2QIdO8gbT9dImR484FzgfE8RztnwiKYluZlrh7TA==", "220018bc-af9e-4427-beee-37a71124f944" });

            migrationBuilder.InsertData(
                table: "LendingHistory",
                columns: new[] { "Id", "BookId", "LeaseActualEndDate", "LeaseProjectedEndDate", "LeaseStartDate", "UserId" },
                values: new object[] { -1, 1, null, new DateOnly(2024, 10, 15), new DateOnly(2024, 10, 1), "17ca3b5e-d8ba-4db1-a5a1-09915c95b06b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LendingHistory",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7019dd9-9663-422d-a8cc-fd30c72ed31f", "AQAAAAIAAYagAAAAEA+RStZas5OjJlJxPtflIUlRyBrz870n2VFORNU6+Q2S6RQVUazNP6aE9R6PsuCzzQ==", "af888e54-7281-4acc-b506-9e2a75a6ad3b" });
        }
    }
}
