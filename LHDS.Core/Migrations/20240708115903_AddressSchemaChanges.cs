// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddressSchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DataSetSpecifications_DataSetId",
                schema: "Configuration",
                table: "DataSetSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_Address_IsErrored",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "IsErrored",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "JsonPostalAddress",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PostalAddress",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.EnsureSchema(
                name: "Addresses");

            migrationBuilder.RenameTable(
                name: "ResolvedAddress",
                schema: "UPRN",
                newName: "ResolvedAddress",
                newSchema: "Addresses");

            migrationBuilder.RenameTable(
                name: "Address",
                schema: "UPRN",
                newName: "Address",
                newSchema: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Processing",
                schema: "Addresses",
                table: "Address",
                newName: "IsSynced");

            migrationBuilder.RenameColumn(
                name: "IsNormalised",
                schema: "Addresses",
                table: "Address",
                newName: "IsProcessing");

            migrationBuilder.RenameIndex(
                name: "IX_Address_Processing",
                schema: "Addresses",
                table: "Address",
                newName: "IX_Address_IsSynced");

            migrationBuilder.RenameIndex(
                name: "IX_Address_IsNormalised",
                schema: "Addresses",
                table: "Address",
                newName: "IX_Address_IsProcessing");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                schema: "Configuration",
                table: "Suppliers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId_OurSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "DataSetId", "OurSpecificationVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId_SupplierSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "DataSetId", "SupplierSpecificationVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_DataSetName",
                schema: "Configuration",
                table: "DataSets",
                column: "DataSetName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_UPRN",
                schema: "Addresses",
                table: "Address",
                column: "UPRN",
                unique: true,
                filter: "[UPRN] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Name",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_DataSetSpecifications_DataSetId_OurSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_DataSetSpecifications_DataSetId_SupplierSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications");

            migrationBuilder.DropIndex(
                name: "IX_DataSets_DataSetName",
                schema: "Configuration",
                table: "DataSets");

            migrationBuilder.DropIndex(
                name: "IX_Address_UPRN",
                schema: "Addresses",
                table: "Address");

            migrationBuilder.EnsureSchema(
                name: "UPRN");

            migrationBuilder.RenameTable(
                name: "ResolvedAddress",
                schema: "Addresses",
                newName: "ResolvedAddress",
                newSchema: "UPRN");

            migrationBuilder.RenameTable(
                name: "Address",
                schema: "Addresses",
                newName: "Address",
                newSchema: "UPRN");

            migrationBuilder.RenameColumn(
                name: "IsSynced",
                schema: "UPRN",
                table: "Address",
                newName: "Processing");

            migrationBuilder.RenameColumn(
                name: "IsProcessing",
                schema: "UPRN",
                table: "Address",
                newName: "IsNormalised");

            migrationBuilder.RenameIndex(
                name: "IX_Address_IsSynced",
                schema: "UPRN",
                table: "Address",
                newName: "IX_Address_Processing");

            migrationBuilder.RenameIndex(
                name: "IX_Address_IsProcessing",
                schema: "UPRN",
                table: "Address",
                newName: "IX_Address_IsNormalised");

            migrationBuilder.AddColumn<bool>(
                name: "IsErrored",
                schema: "UPRN",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JsonPostalAddress",
                schema: "UPRN",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalAddress",
                schema: "UPRN",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId",
                schema: "Configuration",
                table: "DataSetSpecifications",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_IsErrored",
                schema: "UPRN",
                table: "Address",
                column: "IsErrored");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address",
                column: "UPRN");
        }
    }
}
