using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLendingHistoryAndWaitingListTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LendingHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeaseStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaseProjectedEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaseActualEndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendingHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LendingHistory_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaitingList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitingList_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaitingList_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8584d4c0-c592-408b-b8e6-8f1723d73825", "AQAAAAIAAYagAAAAEBpEg2VV9iMBtaHFsi7J8j7PwHpJN+rkz/PNKnUzyQAYalAX2KEJzlGjwTnbzHUEaQ==", "5fa1a3ed-6e14-4daa-9d6f-58f109453ee8" });

            migrationBuilder.CreateIndex(
                name: "IX_LendingHistory_BookId",
                table: "LendingHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LendingHistory_UserId",
                table: "LendingHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingList_BookId",
                table: "WaitingList",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingList_UserId",
                table: "WaitingList",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendingHistory");

            migrationBuilder.DropTable(
                name: "WaitingList");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "346af5ae-f86a-4232-b884-b3780db6c968", "AQAAAAIAAYagAAAAEC9LT1UEgf3MxdGqOGtcGRt062D8ZZtphc6VQk871lUf44ntQF4jJQTh/YMngKthNA==", "10cbd096-2d38-4211-942b-0360f9ab0a38" });
        }
    }
}
