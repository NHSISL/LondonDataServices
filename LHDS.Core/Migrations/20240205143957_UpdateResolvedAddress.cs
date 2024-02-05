using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResolvedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchAlgorithmEnum",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchAlgorithmUsed");

            migrationBuilder.AddColumn<int>(
                name: "BestMatchType",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestMatchType",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "MatchAlgorithmUsed",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchAlgorithmEnum");
        }
    }
}
