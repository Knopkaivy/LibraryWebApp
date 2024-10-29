using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookAddIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f838e5e6-ecd4-495b-ac70-6687c8b61290", "AQAAAAIAAYagAAAAECJJdI6Glndx0s8kDLDnoqOB5OFxODC3jFIIXmDx3820tQwmm2h+weupvmWXhEdJmA==", "0dd6acd2-0799-4f56-8938-e60f9d1fc514" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Book");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c2a4bda-2abb-4ddd-865e-68fb98d337a3", "AQAAAAIAAYagAAAAEJI94pUuOCR0wKo3kHCCnOevzo2QIdO8gbT9dImR484FzgfE8RztnwiKYluZlrh7TA==", "220018bc-af9e-4427-beee-37a71124f944" });
        }
    }
}
