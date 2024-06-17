using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class IngestionTrackingRetry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDownloaded",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessing",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDownloaded",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "IsProcessing",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                schema: "Ingestion",
                table: "IngestionTrackings");
        }
    }
}
