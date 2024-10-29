using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class WaitingListRemoveIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WaitingList");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0bf31339-4e34-4ad1-a34b-e143fd1d5a4a", "AQAAAAIAAYagAAAAEPIKTx/Fjq9dCknD8I5j4nvprW3jceaS5PNOBMiIEdYDvE2aVDdhgiMqtIqAWeuFYQ==", "aee87d33-3465-4583-ab77-3993b4ea6082" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WaitingList",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "80c4e17f-70dd-4aaa-bbc0-ac3fd88682bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7bc4edc-9e16-4e5c-8f4f-f673b1c5ae17", "AQAAAAIAAYagAAAAEGxm2kAi+a69u2M8iXAihnX871KfJyzIxiKhu+dLx40iaTnejXK0c5U+yT2TdgsVng==", "6f88a505-3614-4bdd-bbe0-61c26a29f9e6" });
        }
    }
}
