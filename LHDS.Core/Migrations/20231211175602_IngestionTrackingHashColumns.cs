using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class IngestionTrackingHashColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DecryptedFileSha256Hash",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedFileSha256Hash",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecryptedFileSha256Hash",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "EncryptedFileSha256Hash",
                schema: "Ingestion",
                table: "IngestionTrackings");
        }
    }
}
