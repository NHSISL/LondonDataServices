using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRecordCountFromIngestionTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordCount",
                schema: "Ingestion",
                table: "IngestionTrackings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecordCount",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
