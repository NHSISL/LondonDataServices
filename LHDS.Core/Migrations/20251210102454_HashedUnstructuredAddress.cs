using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class HashedUnstructuredAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashedUnstructuredPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "char(32)",
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
