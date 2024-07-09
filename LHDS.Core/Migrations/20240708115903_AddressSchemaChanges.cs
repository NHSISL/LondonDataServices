using System;
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

            migrationBuilder.DeleteData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"));

            migrationBuilder.DeleteData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"));

            migrationBuilder.DeleteData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"));

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

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "Suppliers",
                columns: new[] { "Id", "CanDecryptIngestionTracking", "CanDownloadIngestionTracking", "CreatedBy", "CreatedDate", "Description", "FriendlyName", "Name", "UpdatedBy", "UpdatedDate", "canRelandIngestionTracking" },
                values: new object[] { new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), false, false, "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Emis Supplier", "EMIS", "EMIS", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSets",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CollectedBy", "CreatedBy", "CreatedDate", "DataSetAliases", "DataSetAuthor", "DataSetName", "DataSourceType", "IsActive", "SpecifiedBy", "SupplierId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EMIS", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PrimaryCareEMISDEV", "EMISDEV", "PrimaryCareEMISDEV", "PrimaryCareEMISDEV", true, "EMIS", new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CreatedBy", "CreatedDate", "DataSetId", "DateImplemented", "DateReleased", "DateSuperseded", "EntityChangeSynchronisation", "IsActive", "IsMultiAuthorPerBatch", "IsPublished", "Notes", "OurSpecificationVersion", "PresededById", "SupersededById", "SupplierSpecificationVersion", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "", true, true, true, "This is a test dataset specification", "1.0", null, null, "7.0", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

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
