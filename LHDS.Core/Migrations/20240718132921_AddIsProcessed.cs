using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIsProcessed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Matched",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "MatchedWithAssign");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "MatchedWithAssign",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "Matched");
        }
    }
}
