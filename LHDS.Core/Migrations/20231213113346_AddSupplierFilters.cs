// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDecryptIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "canRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CanDecryptIngestionTracking", "CanDownloadIngestionTracking", "canRelandIngestionTracking" },
                values: new object[] { false, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDecryptIngestionTracking",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CanDownloadIngestionTracking",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "canRelandIngestionTracking",
                schema: "Configuration",
                table: "Suppliers");
        }
    }
}
