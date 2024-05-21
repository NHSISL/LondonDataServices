using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedResolvedAddressSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchedWithPostalAddress",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedPostalAddress");

            migrationBuilder.RenameColumn(
                name: "MatchedWithJsonPostalAddress",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedJsonPostalAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchedPostalAddress",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedWithPostalAddress");

            migrationBuilder.RenameColumn(
                name: "MatchedJsonPostalAddress",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedWithJsonPostalAddress");
        }
    }
}
