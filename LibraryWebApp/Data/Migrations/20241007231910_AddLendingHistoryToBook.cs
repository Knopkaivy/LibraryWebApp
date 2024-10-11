using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLendingHistoryToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7019dd9-9663-422d-a8cc-fd30c72ed31f", "AQAAAAIAAYagAAAAEA+RStZas5OjJlJxPtflIUlRyBrz870n2VFORNU6+Q2S6RQVUazNP6aE9R6PsuCzzQ==", "af888e54-7281-4acc-b506-9e2a75a6ad3b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8584d4c0-c592-408b-b8e6-8f1723d73825", "AQAAAAIAAYagAAAAEBpEg2VV9iMBtaHFsi7J8j7PwHpJN+rkz/PNKnUzyQAYalAX2KEJzlGjwTnbzHUEaQ==", "5fa1a3ed-6e14-4daa-9d6f-58f109453ee8" });
        }
    }
}
