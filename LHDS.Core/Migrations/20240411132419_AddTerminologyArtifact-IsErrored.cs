using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTerminologyArtifactIsErrored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsErrorred",
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
                name: "ErrorMessage",
                schema: "Ontology",
                table: "TerminologyArtifacts");

            migrationBuilder.DropColumn(
                name: "IsErrorred",
                schema: "Ontology",
                table: "TerminologyArtifacts");
        }
    }
}
