using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedColFromSuppliers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CanRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
