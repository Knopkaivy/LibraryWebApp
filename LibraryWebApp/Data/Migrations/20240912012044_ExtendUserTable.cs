using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "346af5ae-f86a-4232-b884-b3780db6c968", new DateOnly(1991, 1, 1), "Admideus", "Admin", "AQAAAAIAAYagAAAAEC9LT1UEgf3MxdGqOGtcGRt062D8ZZtphc6VQk871lUf44ntQF4jJQTh/YMngKthNA==", "10cbd096-2d38-4211-942b-0360f9ab0a38" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02e70e08-1638-4a1f-b14a-56384372ff51", "AQAAAAIAAYagAAAAEEjAtFZIiEOhTNnL1jy0aDsdP5MR7Up8OR4AQmVn8RX7sgpAhXtKmtv2bZw7LhcoIw==", "1048279a-a78a-479f-b8c8-eeb6eb1d23b9" });
        }
    }
}
