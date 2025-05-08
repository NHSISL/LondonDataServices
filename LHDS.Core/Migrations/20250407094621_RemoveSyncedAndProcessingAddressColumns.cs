using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSyncedAndProcessingAddressColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Address_IsProcessing",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_IsSynced",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "IsProcessing",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "USRN",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "USRN",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                schema: "Addresses",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                schema: "Addresses",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Address_IsProcessing",
                schema: "Addresses",
                table: "Address",
                column: "IsProcessing");

            migrationBuilder.CreateIndex(
                name: "IX_Address_IsSynced",
                schema: "Addresses",
                table: "Address",
                column: "IsSynced");
        }
    }
}
