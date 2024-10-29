using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookRemoveLendingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7bc4edc-9e16-4e5c-8f4f-f673b1c5ae17", "AQAAAAIAAYagAAAAEGxm2kAi+a69u2M8iXAihnX871KfJyzIxiKhu+dLx40iaTnejXK0c5U+yT2TdgsVng==", "6f88a505-3614-4bdd-bbe0-61c26a29f9e6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f838e5e6-ecd4-495b-ac70-6687c8b61290", "AQAAAAIAAYagAAAAECJJdI6Glndx0s8kDLDnoqOB5OFxODC3jFIIXmDx3820tQwmm2h+weupvmWXhEdJmA==", "0dd6acd2-0799-4f56-8938-e60f9d1fc514" });
        }
    }
}
