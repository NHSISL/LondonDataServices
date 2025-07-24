using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUPRNKeepUSRN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UPSN",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "UPSN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "USRN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "USRN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "UPSN");

            migrationBuilder.AddColumn<string>(
                name: "UPSN",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
