using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Book");

            migrationBuilder.CreateTable(
                name: "UserViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserViewModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5323d7a2-c3c8-4493-9744-18f5b7045522", "AQAAAAIAAYagAAAAEM+6hC8mWgtM5zjiIAaq3Dsr4JHWQk4oBw2p9tKM7GP3QaHmb9dMi7xfMS18YcFNdA==", "5c4bffcb-cc97-4784-ae9b-f63997d9be85" });

            migrationBuilder.CreateIndex(
                name: "IX_UserViewModel_UserId",
                table: "UserViewModel",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserViewModel");

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
                values: new object[] { "0bf31339-4e34-4ad1-a34b-e143fd1d5a4a", "AQAAAAIAAYagAAAAEPIKTx/Fjq9dCknD8I5j4nvprW3jceaS5PNOBMiIEdYDvE2aVDdhgiMqtIqAWeuFYQ==", "aee87d33-3465-4583-ab77-3993b4ea6082" });
        }
    }
}
