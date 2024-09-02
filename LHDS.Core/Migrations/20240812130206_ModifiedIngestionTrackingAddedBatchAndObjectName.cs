using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedIngestionTrackingAddedBatchAndObjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Batch",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjectName",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Batch",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "ObjectName",
                schema: "Ingestion",
                table: "IngestionTrackings");
        }
    }
}
