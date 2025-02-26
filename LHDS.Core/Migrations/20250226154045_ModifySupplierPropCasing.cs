using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifySupplierPropCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CanRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "CanDecryptIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CanRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "CanDecryptIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
