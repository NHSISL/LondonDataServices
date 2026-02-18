using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class HashedAddressToByteArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "HashedUnstructuredPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "VARBINARY(16)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedUnstructuredPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress");
        }
    }
}
