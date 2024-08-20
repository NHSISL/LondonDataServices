using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class TerminologyUserDownload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDownloadedForUser",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForUser",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloadedForUser",
                schema: "Ontology",
                table: "TerminologyArtifacts");

            migrationBuilder.DropColumn(
                name: "IsForUser",
                schema: "Ontology",
                table: "TerminologyArtifacts");
        }
    }
}
