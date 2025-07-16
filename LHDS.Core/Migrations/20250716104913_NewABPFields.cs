using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class NewABPFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XCoordinate",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YCoordinate",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XCoordinate",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YCoordinate",
                schema: "Addresses",
                table: "Address",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "XCoordinate",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "YCoordinate",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "XCoordinate",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "YCoordinate",
                schema: "Addresses",
                table: "Address");
        }
    }
}
