// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UprnModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UPRN");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UPRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UPSN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    OrganisationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DepartmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubBuildingName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuildingName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DependentThoroughfare = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Thoroughfare = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DoubleDependentLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DependentLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostTown = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressExtractionAudit",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressExtractionAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressLoadingAudit",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressLoadingAudit", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016b7c2a-3cb9-4433-a79e-b55ca988eb57"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016f00d9-f56e-4610-a38d-a3c49023d4c1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("01abe9ac-bbd0-41e7-a8c3-b7c1a967cd1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("024eb41e-404f-461f-ad10-fcb9a6c37fe4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("02fdb2c0-8fb5-4a4a-b1e0-68614da128e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("04c59f48-ba31-4809-94bb-f4f5e1cf4460"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("053eade2-698d-4cbd-b84b-213adb1ea3f5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("05803762-b255-48b9-bdae-802e7a6aaf05"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("07c3cca3-7eb1-451e-8087-ee1f2aa733ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("08ae5632-37ad-4f2a-bfd2-a3ef3c73d6cb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b1725d-18d8-4222-8c43-a4bea3ba8b06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b6077f-74a6-4cc0-9992-0c7723d81416"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09f8009d-8453-4ce5-b7b4-8ed6eb4b2d31"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09fb5c58-0336-4b20-b521-83e5cd8b6b49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a5fc198-19b8-43ae-b798-45506c385e20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a946e6b-f495-4cd4-8639-bc17173d1864"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ad1dd7b-4d2a-417e-a6b2-6135af7c19b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bbbd975-da17-4340-95ae-34646282eba2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f1e1-3583-4ae2-8eac-6df87422eb79"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ca7ae3a-036d-49a4-bb59-4cf3b5435ce3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0cfc4aff-ce97-4c48-8258-0ba55e63c49c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0e166c8f-eb6c-43d0-b12c-f49d44953e6a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ef487f9-476e-4ec8-899d-541478430e16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f37bbe4-afbf-474b-815e-f0dbdb653540"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f5f262d-55d1-4611-9b1f-355ec4aab0d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0fe408cb-5610-461b-8260-510814bc54bb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1091b3d3-a4a8-44b8-b4e5-c10c1014dc3f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1481245d-c122-4890-b5d4-ae87ff63c62a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15096c24-3ea9-4f19-a7a0-ee4c5a09dd12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15422b24-e865-4db5-8536-3cedb69e3efc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15d5d359-d192-43ff-b7bd-b4ad0406146e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("16143ebe-976a-4a9e-805d-8f85265e1003"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("17b7fdc6-616c-4fa7-8ee6-f970443d0c11"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("18d0f511-1978-46cc-9721-0475433bccc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1b3388cf-2e88-431e-815f-b8eef77568f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c34861b-aec0-4508-99a9-c4f79a76abfc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c71fe1b-41f4-4e7c-aa5f-c1ecbcc7455a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d11df66-c4a7-4e85-84bb-77a5fe923590"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d661e5a-fcf8-4dec-8634-07f5f6c3e741"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1e11b982-faad-4e5c-a8bd-95f1d4b5c648"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1f381e8f-19e4-4685-a84b-6c7f41ca0480"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("223b3cc1-56f6-44ed-89cf-b13b06e49e99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("233b0b2f-4620-4adb-8107-8a7e3fea5b1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("23d3274a-f5e4-47a2-8ccb-a4d5d5aa84b2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("25cb156f-86d0-4c68-9583-6ddbe08042cf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("261902dc-c53e-4017-b0ab-7e350ce62c7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("26331b24-6f9f-4f6f-bfcb-c8ae81dca9a8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2655335d-4a67-4be7-a8f1-860a7c7aec20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28246209-2a86-456c-97a5-00eb5ed89276"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28ce4c02-8c50-465c-b2e9-a6065eb5d522"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2afb470a-7b78-4a3f-afab-fc817dcae6e8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b4b318a-f79f-4eb6-9c8d-90bbc1a5baa2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b762785-2318-4407-b52b-c93142fc067f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2c1c993a-72f3-4bf3-ad8e-cbfcc0cdd323"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2cb2c37e-677c-4918-93b9-bdb4a18fdc99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2dc110e6-417f-41d1-97aa-e1d8bcae2126"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2de218bf-f65c-44c2-8b97-e813931aa669"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2e7f0550-7ddd-4035-899f-b5b0c01a2f43"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2f2df5d0-141e-4cf8-83a3-13c520078e51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2fe17d9f-3ff3-41a9-a75f-f9776e8278f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("310cdb68-574f-4904-ac15-589db18d93a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("31b8a2cb-7ea8-4212-ba08-93f07f52a926"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3277f9a2-be19-44f7-a320-4e57b08186c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("32f7d725-972f-4c61-9cd7-120c4575f6da"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3457368f-3486-4598-b75e-6c4862acc273"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3493ddcc-e3db-4ab5-bc2c-bd0c53910fd9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3504618b-fbdf-4d1a-8ce3-4beaeb9f36b1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("352d7e99-e0a3-417a-a9f4-41764c8037d3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("354a00f1-919f-46e5-bc27-558864dd6282"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("35e8f22d-40e3-454f-8c82-af50e81227e5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("37ca1513-b100-4af7-a7dc-5535c9041b60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("39609eae-9ca8-4a8d-93a5-52d28889e80b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a41011e-8561-47f7-9efd-f98a2b121c01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a6933ca-e959-4cb9-a5c7-7c85776cd2c3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3c38425d-ca08-458e-8aa6-6dbde22d075f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3d1c757f-4c5c-41c4-9431-3d76f54405fb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f07700f-b59e-4e13-b8a6-e04872461ba3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f51ce5d-7894-4332-9ce2-28f9bfffd9cc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3fbc97b7-a830-4e59-a534-18e27c1659de"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("405afb0c-1252-4c8e-b1d7-193dc2ea61ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("40a02930-815a-4396-9893-7138128667d0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("424e2ade-0b15-4014-bd24-0e66e50c69a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("42c1ad57-365d-43c8-a746-503c9b723060"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("43d87e04-fddf-406d-9172-adffc4295be0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4737f612-058e-4491-829f-a336fb9136e1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("479b24dc-8673-4a71-ac3b-2b45d1669614"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("47c4a842-a333-4034-9741-a8aea34e6159"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("48e9e1a9-9233-495e-a5c5-c1398a433ad8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a71dd62-bef0-4d7b-a967-4ae93751a69b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a79190f-0271-4e66-936f-8d4bfd10a5ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4affbb36-dbe5-483b-bdb9-3b21565e0658"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4baf1d59-6c43-4d72-a1f9-318e44cf1bae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c55b3a2-b325-4835-939e-5e3ef835bf66"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c80e258-93f8-4631-b83b-d5a1254bbdca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4cdebe09-d9ab-4229-91e2-4ca9e6899089"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4d556e6a-4037-4c29-a40d-3fe284679693"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4e3e8c9e-58ed-4aab-acf8-b969e8d44cd5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb23b6a-a682-482d-b9c3-689f29d1856a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb79c3b-df7a-4c59-9d97-ac930ca623e6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4ffb768c-34f2-4aea-ab97-497a3d5c9e32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50557d75-1849-4528-8b57-68a0eebb4877"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50c22935-6a40-4b97-b7d5-e7acd32f7e19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("52f25a39-d50f-4e0e-9dd5-729c7f9c3fee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("53dbbec5-311a-49f3-9dc7-143b9f150eab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("54419442-e140-4642-ab30-af6e2dd74228"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("546f7053-f243-43f2-8479-c97165c1c97c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("556326f1-54e5-4c38-96ff-2a8782d03f59"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("559aef93-6209-456a-8b5c-d859e8c00e49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56f2ea58-47f0-4229-8c22-b101d390bd7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56faac0a-ccd0-4019-9581-108006412c95"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5712c2a5-7e44-4c16-8e98-1d647331f00d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("576c97af-fde1-408d-9042-b75448adc98d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("578e46f1-fb0b-4766-adb1-3204d0ee92c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5870d3d6-c36d-444a-bd48-0ee20b73c8dc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ae2fff6-23ec-46cc-a10e-7dd3d0c5cd1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5b3ef045-3abc-48e2-8198-c31018c99481"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ddcf642-c337-471b-bccf-d7feebbd417e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5dfa3bcf-5f87-4ae7-82af-f4467905e4c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("61e24c11-d0d9-4e4c-9ba8-f44aab8bd573"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("62d21d9d-878c-4d7f-a814-feca5a481cee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6307fe1e-287a-4e0d-b949-eb8696364f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6312a346-339e-43f1-bf4e-ab46a2669f80"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("635198f6-16c4-4033-a433-58cf962ffa6b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63c6725f-d1d1-4349-8320-275874d68594"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ce742c-4532-4211-9f8d-32ac44e1d557"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ef80ea-feb3-46e3-ae65-493ae81df639"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6416ef97-03b3-4a28-9fcf-195b92d5dc6e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("647e5d2d-589a-4caf-b777-2d3a8122cd2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("64e88c5b-31a0-4857-b96e-342b71572897"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("665021ae-e50e-4874-a502-7189380d5f53"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6694a4bb-7729-43d0-8b66-e904d79cc2b3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("674ced77-2991-46c0-8e65-abe2fe966c03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6828c5b8-50fe-41a2-8b69-6eaa22fd86f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("689f2c1c-6a26-4daa-aff2-2d223def8b52"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6a4af2a2-ed38-4e42-9e98-1d630f8c396d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6aacbf40-a3cc-4993-bb1a-3c92aa56366d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6bc62b7a-c062-48a1-846a-127626b977f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6d5c31f4-56f5-4cf3-a875-ad137c8293d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6e37a8c1-9a0d-4cce-a268-724bd1cd3d17"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f328079-ecd0-4b2e-8b37-e4f40f228f9a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f7779fe-1ecc-4da1-8647-51226936afc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6fac827e-b9bd-4ca2-90c4-6ec500a05e03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70de2594-ecea-41ea-8fac-b0302c2b5e65"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70eb906b-851b-4278-9540-2badcc279e10"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7168fb0c-1142-4924-b27d-d39831a75037"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("716fab26-5b55-4d46-a4de-10873d0625b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("71c34eec-9b5f-49c7-a3be-ace36b45f15a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("72b1c7a9-b6d5-40a7-a699-bf6598047882"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73b108e5-328c-4160-95ba-c96bc45a73d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73fd24a5-dddd-4837-8c9e-027cec3175f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74324406-77e4-4f78-b486-5ce18ecce5e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74836307-2d5d-46b6-a350-7fdf1a9bcee2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("754b4547-831d-48f5-a97a-efdd0b4cadc8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("766fde1a-e0f4-4a06-a565-749f0e62a512"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("771145fc-579e-4452-b331-11329dafab55"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("77517652-b41d-4954-bd95-6fa3554c03cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("778959a1-4baa-4f70-863f-92b4db0b1e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a3952d1-f745-4c48-ade4-a7a3483ac348"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a52a66a-759d-4e7b-b0f7-d5a48c5fcba7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7d064556-e6b3-410a-801e-8682b2bb75c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7fef5aaf-0232-4907-88ad-3d58e10c15d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("80d0ff9c-0d05-4727-a603-595dccd73f4b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81062b69-2d5e-4ff3-abc8-8b70fab4cc38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81423693-49b9-4554-a113-5db584f79a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8170d54f-b6c8-4bbc-9763-92b374544edd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81b1e3f7-525e-4b24-8e4b-1342909ef6cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8434011f-6ae2-419c-a77e-220dfb21c028"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("860c0937-8adf-4860-a7c7-5791aa8f7f19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8617a57c-d899-4320-b1e2-88c4b7704072"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("86668055-7b54-4cd3-aa7d-bfc0dc6b223e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("874eef90-39de-4367-a482-5f4d8c9d6f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("87af5a28-4975-4e91-8665-daabc185a403"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8bcd3442-31bd-40c6-a246-de59da0fe2ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c0b89fe-11fb-4196-a246-17a17f4592d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c5eb361-3fb3-478e-afa2-97970139dd69"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c9be888-fbcb-4394-bc9e-3e27571254fc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("924df98b-9eb0-4585-9376-87dbbda62e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("93111020-404c-4712-84cf-8febfda90bda"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9630a6a7-d51e-4912-82ec-3f9372dfbbd1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("969c634e-3f3a-419a-8526-e10edbd8c5ca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("97357fcd-5559-4a09-a7ac-901e3ef851bf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9836be8a-a6d8-40fa-968e-c228f89bb914"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("986291f1-06f6-40b0-81db-ca076898991c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("99016a1e-3bed-4399-bc4e-203e6fcd8a5a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9976bd33-0939-4751-8906-a4dfa5383329"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9b109e35-f3f4-4cc4-b260-531ae5baa6b9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9df91660-cf3c-41bf-bf99-2d4c95f1eb50"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9e620384-0990-4704-b3ee-1414ad4b5d8f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9eb802bb-91fd-4fbd-9d1b-e0a33f363e62"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a037fb68-185f-404c-a68d-b5191fcde40f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a0c8afb7-71e9-49ac-97e4-e30434d41bd4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a377d9c8-b36c-4398-b94d-dffd2b9b7e2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a4ecc6d6-aadd-4fdd-b2d8-211bffe8a60a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a6cb4b9e-865b-4a10-8b27-fc6e18053946"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7ced9df-e3f3-421c-b52e-e851561612cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7fc976d-e924-4b1a-80ba-25fa11c6eada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a8181d58-e6fb-4d84-9ba8-1d3426d63b25"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a9d7e3a9-1d94-48bf-a6b6-f2b4ea642c6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("abbd7ea3-5e9f-429b-9bc9-2eac05e365b0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ac4fd120-5184-43d2-bcbc-3aa661029deb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("acf5ce30-c463-45e1-9b36-0ae006ebdedf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ada316d2-d3bf-4f9e-a768-bc2485a08ada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b18fb01b-496b-4c19-b834-ccfb9247fb16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b1f86138-bd5e-4078-911b-4d81c78e4414"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b296bea3-8590-4c82-8060-87feed620d5f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b482e424-abfd-46ab-8919-015fe2cede4e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b5533594-0720-40fd-b3b2-52e3497b8b40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b586ac34-995f-4743-82f0-21f1f4e3d76d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b7c55d98-2ca5-498d-83c5-65821f69b0a9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b81b6fa4-1d67-4843-8001-d1236c0c046e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b99ec1be-734e-4f3f-b1d2-52dffc9ce612"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ba5c3b79-507e-4afd-bb65-641c9d859763"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bc79b37f-d82d-4f32-8e1f-9834734f114e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bce7924e-ffbb-4434-af88-e3b147b502a7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bd95782e-0020-4fa8-ac92-ff34a5438c2a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("becfbad6-9da2-48e7-8841-82609bb8cac9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bf4b78f7-863c-42d7-9465-fa11a6a9b175"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bfc76c18-4434-4aae-924a-d8dd126e4014"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c0cb789d-f745-4a0d-b166-763c499563c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c43440ae-99c5-4722-84f9-29dc752e81d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c5763f60-c5de-42d1-b97e-d78745e281f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c6c1fe44-2147-41ca-8100-48c1179545a0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8aad664-4329-40df-bdae-404d16ad28e0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8b369a5-b9c1-48f7-9aeb-e56e39eea7ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c95f54b9-743a-43c5-9336-753646b2bae0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cceb2b1b-d848-41a2-b987-1c256c1caf3d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cede99f7-0fac-416c-aca0-359b1ceb6665"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf34f086-c4f2-49c2-9b12-8cec89f76d73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf86d807-0488-4fdc-86cd-fbc1849c2232"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cfc21067-9cb1-4e8f-bd06-698befaad209"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d019d451-4ef0-4a26-ac7c-3fe0040a03f8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d03dff77-9c74-49bb-a5a7-10f00a52eadf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d04b3225-4455-4dbf-879c-52521881ba32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d0eb085d-9415-4f52-aee2-3d2631ee17e2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d10f35d9-7528-41ed-8362-25a13e3d19c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d1569f7a-17b0-4499-86bc-8da3f62d78df"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d21fb3f4-063a-464c-8732-c5a8aeca001d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d28d60b7-a11f-420d-9b78-8978221a12b7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d3c64bb3-2e70-4eac-b1fb-5bf4fdc374c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d472d4c8-b215-4477-8f0d-687e8f698158"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d48e3d13-de43-429a-bbc3-0cdcc1ca8ea1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d502b3f2-936d-449b-90cd-a783fb2942f4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d57947e1-16c0-4b9d-9573-3d2767815451"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d59b6e17-2905-46f7-977b-545cdbf9251e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d5b77b5b-ace9-4c54-af3e-bae4449696c8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7919a88-1042-4951-991d-be49aba34367"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7ab6445-47e1-4602-8c29-66d500b5ca51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d927c02d-5ebc-4b7b-9b7f-f7fab5ca3bc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d93ff951-cc6d-4c57-addf-cf34965074c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d98bab5e-1b3d-4138-b97c-b71db86d30c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("dbe48e80-b8d1-4140-b030-8a1c348ac3c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("de36b74c-6770-4a9a-8e53-3a032d3170af"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("deed3198-0bfa-4313-9919-64090878ff40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0b42e50-0db1-440a-b392-7056af38f492"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0f45b74-6c7a-4c6f-b913-41d887443cd3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e1a611db-e710-467c-9404-de8e82dfcdc7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e4866ef8-7a9f-49ea-af3d-2f2adc388255"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e49dcd0d-3820-4c45-9990-3b799d0b733b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e5be72d6-38eb-44c4-b087-6e1bc0120733"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e6056baa-29fe-43b4-bc21-e0c8a89ec112"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7274bb8-ab3c-4138-b091-9ddddef4499e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7300f3a-c370-43d5-b268-44286a302c60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e73c2a6a-ca1e-4149-b692-e8fb3cc67d48"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7fd9c3b-207a-426d-829c-5864087b0ceb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e88eef15-2e0c-42c2-a5f5-4cba09df8b7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e964019e-39bf-4477-ba06-9cff9d3e44ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eae2509e-0b08-42c6-9123-205c725291a2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb447e65-d19d-491e-b4bf-a0517a64b79b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb5281e2-efc5-444c-a92d-1a2603721b4f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb7554df-f80e-4194-886d-bee5babdd62c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eba41453-c8c2-491d-bb86-9598de7ffab2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ec289217-c6b7-4837-8b8a-2cbc9fbc5df0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ed375f05-4451-42fa-bebe-96c194fcfddf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ee736134-e8a9-4f6b-9057-aefe27a222d2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ef4b1a02-bf0c-4aee-bf72-0fb21a303bc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f30ae7c6-9f2d-42d9-9ba4-9a44a658fb6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f318741a-31bf-41ad-9e30-627955fbcefe"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48d909e-a7ac-4990-b8da-2bc87429e7a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48df6b4-ada9-4504-9a4a-ef68a0edaf8d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f4f8d8c5-735c-42f3-b534-bf92030606d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f5c0457d-d3a6-4b87-931a-0ccdc4bdb343"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f715b6bb-91a0-452a-a589-0c4500aa3a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f7c81ff9-6224-42ea-8080-eed6479b334a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8924f7d-95dc-4e9f-b092-d0ad5046c1a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8d3db4c-f63f-4c2a-84bf-174411fe1e22"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("fb03709a-af48-452d-b711-d5990db0fa38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ff23c77a-22fa-4e27-86f6-a71cb4e43e36"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("e44e12a4-df37-401e-afc9-08024be3991a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "UPRN");

            migrationBuilder.DropTable(
                name: "AddressExtractionAudit",
                schema: "UPRN");

            migrationBuilder.DropTable(
                name: "AddressLoadingAudit",
                schema: "UPRN");

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3469), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3470), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3444), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3445), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016b7c2a-3cb9-4433-a79e-b55ca988eb57"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3654), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3655), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016f00d9-f56e-4610-a38d-a3c49023d4c1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4327), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4327), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("01abe9ac-bbd0-41e7-a8c3-b7c1a967cd1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4097), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4097), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("024eb41e-404f-461f-ad10-fcb9a6c37fe4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4653), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4653), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("02fdb2c0-8fb5-4a4a-b1e0-68614da128e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4373), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4373), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("04c59f48-ba31-4809-94bb-f4f5e1cf4460"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3953), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3954), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("053eade2-698d-4cbd-b84b-213adb1ea3f5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4357), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4358), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("05803762-b255-48b9-bdae-802e7a6aaf05"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4585), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4586), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("07c3cca3-7eb1-451e-8087-ee1f2aa733ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3706), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3706), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("08ae5632-37ad-4f2a-bfd2-a3ef3c73d6cb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3938), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3938), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b1725d-18d8-4222-8c43-a4bea3ba8b06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3941), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3942), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b6077f-74a6-4cc0-9992-0c7723d81416"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4131), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4132), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09f8009d-8453-4ce5-b7b4-8ed6eb4b2d31"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4155), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4155), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09fb5c58-0336-4b20-b521-83e5cd8b6b49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4777), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4777), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a5fc198-19b8-43ae-b798-45506c385e20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4368), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4369), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a946e6b-f495-4cd4-8639-bc17173d1864"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4027), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4027), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ad1dd7b-4d2a-417e-a6b2-6135af7c19b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4513), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4514), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bbbd975-da17-4340-95ae-34646282eba2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4563), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4563), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f1e1-3583-4ae2-8eac-6df87422eb79"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3788), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3789), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ca7ae3a-036d-49a4-bb59-4cf3b5435ce3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4304), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4304), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0cfc4aff-ce97-4c48-8258-0ba55e63c49c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4275), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4276), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0e166c8f-eb6c-43d0-b12c-f49d44953e6a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4387), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4388), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ef487f9-476e-4ec8-899d-541478430e16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4319), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4319), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f37bbe4-afbf-474b-815e-f0dbdb653540"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4543), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4544), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f5f262d-55d1-4611-9b1f-355ec4aab0d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4422), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4422), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0fe408cb-5610-461b-8260-510814bc54bb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4057), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4058), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1091b3d3-a4a8-44b8-b4e5-c10c1014dc3f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4469), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4470), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1481245d-c122-4890-b5d4-ae87ff63c62a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4780), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4781), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15096c24-3ea9-4f19-a7a0-ee4c5a09dd12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4619), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4619), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15422b24-e865-4db5-8536-3cedb69e3efc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3738), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3738), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15d5d359-d192-43ff-b7bd-b4ad0406146e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4339), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4339), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("16143ebe-976a-4a9e-805d-8f85265e1003"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4706), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4707), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("17b7fdc6-616c-4fa7-8ee6-f970443d0c11"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4482), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4483), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("18d0f511-1978-46cc-9721-0475433bccc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4272), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4272), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1b3388cf-2e88-431e-815f-b8eef77568f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3829), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3830), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c34861b-aec0-4508-99a9-c4f79a76abfc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4112), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4112), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c71fe1b-41f4-4e7c-aa5f-c1ecbcc7455a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4473), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4474), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d11df66-c4a7-4e85-84bb-77a5fe923590"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4656), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4657), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d661e5a-fcf8-4dec-8634-07f5f6c3e741"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3714), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3715), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1e11b982-faad-4e5c-a8bd-95f1d4b5c648"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4015), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4015), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1f381e8f-19e4-4685-a84b-6c7f41ca0480"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4521), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4521), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("223b3cc1-56f6-44ed-89cf-b13b06e49e99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4465), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4465), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("233b0b2f-4620-4adb-8107-8a7e3fea5b1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3806), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3808), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("23d3274a-f5e4-47a2-8ccb-a4d5d5aa84b2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4080), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4081), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("25cb156f-86d0-4c68-9583-6ddbe08042cf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3634), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3634), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("261902dc-c53e-4017-b0ab-7e350ce62c7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4457), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4458), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("26331b24-6f9f-4f6f-bfcb-c8ae81dca9a8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4649), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4649), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2655335d-4a67-4be7-a8f1-860a7c7aec20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4380), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4380), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28246209-2a86-456c-97a5-00eb5ed89276"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4268), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4268), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28ce4c02-8c50-465c-b2e9-a6065eb5d522"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4461), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4462), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2afb470a-7b78-4a3f-afab-fc817dcae6e8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4800), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4801), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b4b318a-f79f-4eb6-9c8d-90bbc1a5baa2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4197), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4198), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b762785-2318-4407-b52b-c93142fc067f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3839), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3839), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2c1c993a-72f3-4bf3-ad8e-cbfcc0cdd323"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4604), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4605), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2cb2c37e-677c-4918-93b9-bdb4a18fdc99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4105), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4105), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2dc110e6-417f-41d1-97aa-e1d8bcae2126"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3670), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3671), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2de218bf-f65c-44c2-8b97-e813931aa669"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3701), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3702), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2e7f0550-7ddd-4035-899f-b5b0c01a2f43"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4786), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4787), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2f2df5d0-141e-4cf8-83a3-13c520078e51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4740), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4740), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2fe17d9f-3ff3-41a9-a75f-f9776e8278f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3870), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3871), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("310cdb68-574f-4904-ac15-589db18d93a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4437), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4438), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("31b8a2cb-7ea8-4212-ba08-93f07f52a926"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4158), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4159), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3277f9a2-be19-44f7-a320-4e57b08186c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3762), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3763), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("32f7d725-972f-4c61-9cd7-120c4575f6da"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3722), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3723), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3457368f-3486-4598-b75e-6c4862acc273"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4773), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4774), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3493ddcc-e3db-4ab5-bc2c-bd0c53910fd9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4717), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4718), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3504618b-fbdf-4d1a-8ce3-4beaeb9f36b1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4231), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4232), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("352d7e99-e0a3-417a-a9f4-41764c8037d3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4762), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4763), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("354a00f1-919f-46e5-bc27-558864dd6282"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3775), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3776), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("35e8f22d-40e3-454f-8c82-af50e81227e5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3650), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3650), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("37ca1513-b100-4af7-a7dc-5535c9041b60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4570), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4571), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("39609eae-9ca8-4a8d-93a5-52d28889e80b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4311), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4312), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a41011e-8561-47f7-9efd-f98a2b121c01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4623), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4623), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a6933ca-e959-4cb9-a5c7-7c85776cd2c3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4691), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4692), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3c38425d-ca08-458e-8aa6-6dbde22d075f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4578), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4578), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3d1c757f-4c5c-41c4-9431-3d76f54405fb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3619), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3620), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f07700f-b59e-4e13-b8a6-e04872461ba3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4213), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4213), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f51ce5d-7894-4332-9ce2-28f9bfffd9cc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4220), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4221), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3fbc97b7-a830-4e59-a534-18e27c1659de"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4392), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4392), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("405afb0c-1252-4c8e-b1d7-193dc2ea61ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3885), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3886), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("40a02930-815a-4396-9893-7138128667d0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4242), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4243), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("424e2ade-0b15-4014-bd24-0e66e50c69a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3957), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3958), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("42c1ad57-365d-43c8-a746-503c9b723060"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3766), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3767), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("43d87e04-fddf-406d-9172-adffc4295be0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4334), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4335), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4737f612-058e-4491-829f-a336fb9136e1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4077), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4077), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("479b24dc-8673-4a71-ac3b-2b45d1669614"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4365), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4365), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("47c4a842-a333-4034-9741-a8aea34e6159"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3822), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3823), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("48e9e1a9-9233-495e-a5c5-c1398a433ad8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4600), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4601), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a71dd62-bef0-4d7b-a967-4ae93751a69b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3756), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3758), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a79190f-0271-4e66-936f-8d4bfd10a5ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3638), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3638), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4affbb36-dbe5-483b-bdb9-3b21565e0658"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3983), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3984), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4baf1d59-6c43-4d72-a1f9-318e44cf1bae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4135), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4135), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c55b3a2-b325-4835-939e-5e3ef835bf66"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4065), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4066), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c80e258-93f8-4631-b83b-d5a1254bbdca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4116), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4116), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4cdebe09-d9ab-4229-91e2-4ca9e6899089"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4235), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4d556e6a-4037-4c29-a40d-3fe284679693"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3921), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3921), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4e3e8c9e-58ed-4aab-acf8-b969e8d44cd5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3835), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3836), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb23b6a-a682-482d-b9c3-689f29d1856a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4453), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4453), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb79c3b-df7a-4c59-9d97-ac930ca623e6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4384), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4384), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4ffb768c-34f2-4aea-ab97-497a3d5c9e32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4430), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4431), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50557d75-1849-4528-8b57-68a0eebb4877"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4190), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4190), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50c22935-6a40-4b97-b7d5-e7acd32f7e19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3678), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3679), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("52f25a39-d50f-4e0e-9dd5-729c7f9c3fee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4003), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4003), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("53dbbec5-311a-49f3-9dc7-143b9f150eab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4743), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4744), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("54419442-e140-4642-ab30-af6e2dd74228"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4092), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4093), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("546f7053-f243-43f2-8479-c97165c1c97c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4790), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4790), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("556326f1-54e5-4c38-96ff-2a8782d03f59"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4559), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4560), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("559aef93-6209-456a-8b5c-d859e8c00e49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4793), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4794), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56f2ea58-47f0-4229-8c22-b101d390bd7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4261), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4261), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56faac0a-ccd0-4019-9581-108006412c95"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4178), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4178), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5712c2a5-7e44-4c16-8e98-1d647331f00d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3730), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3731), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("576c97af-fde1-408d-9042-b75448adc98d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4713), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4714), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("578e46f1-fb0b-4766-adb1-3204d0ee92c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4224), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4225), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5870d3d6-c36d-444a-bd48-0ee20b73c8dc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4406), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4407), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ae2fff6-23ec-46cc-a10e-7dd3d0c5cd1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4582), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4582), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5b3ef045-3abc-48e2-8198-c31018c99481"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3663), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3663), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ddcf642-c337-471b-bccf-d7feebbd417e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3847), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3847), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5dfa3bcf-5f87-4ae7-82af-f4467905e4c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3682), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3683), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("61e24c11-d0d9-4e4c-9ba8-f44aab8bd573"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3865), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3866), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("62d21d9d-878c-4d7f-a814-feca5a481cee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4264), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4265), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6307fe1e-287a-4e0d-b949-eb8696364f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4127), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4128), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6312a346-339e-43f1-bf4e-ab46a2669f80"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3826), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3826), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("635198f6-16c4-4033-a433-58cf962ffa6b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3780), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3780), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63c6725f-d1d1-4349-8320-275874d68594"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4287), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4288), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ce742c-4532-4211-9f8d-32ac44e1d557"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4228), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4228), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ef80ea-feb3-46e3-ae65-493ae81df639"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3753), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3753), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6416ef97-03b3-4a28-9fcf-195b92d5dc6e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4411), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4411), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("647e5d2d-589a-4caf-b777-2d3a8122cd2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3976), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3977), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("64e88c5b-31a0-4857-b96e-342b71572897"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4502), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4502), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("665021ae-e50e-4874-a502-7189380d5f53"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4395), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4396), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6694a4bb-7729-43d0-8b66-e904d79cc2b3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4346), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4346), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("674ced77-2991-46c0-8e65-abe2fe966c03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4597), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4597), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6828c5b8-50fe-41a2-8b69-6eaa22fd86f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4524), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4525), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("689f2c1c-6a26-4daa-aff2-2d223def8b52"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3972), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3972), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6a4af2a2-ed38-4e42-9e98-1d630f8c396d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4747), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4747), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6aacbf40-a3cc-4993-bb1a-3c92aa56366d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4675), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4675), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6bc62b7a-c062-48a1-846a-127626b977f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4353), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4354), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6d5c31f4-56f5-4cf3-a875-ad137c8293d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4536), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4536), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6e37a8c1-9a0d-4cce-a268-724bd1cd3d17"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4250), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4250), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f328079-ecd0-4b2e-8b37-e4f40f228f9a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4123), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4124), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f7779fe-1ecc-4da1-8647-51226936afc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4308), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4308), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6fac827e-b9bd-4ca2-90c4-6ec500a05e03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4193), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4194), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70de2594-ecea-41ea-8fac-b0302c2b5e65"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4257), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4258), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70eb906b-851b-4278-9540-2badcc279e10"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4418), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4419), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7168fb0c-1142-4924-b27d-d39831a75037"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4166), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4166), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("716fab26-5b55-4d46-a4de-10873d0625b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4046), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4047), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("71c34eec-9b5f-49c7-a3be-ace36b45f15a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4645), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4645), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("72b1c7a9-b6d5-40a7-a699-bf6598047882"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4108), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4109), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73b108e5-328c-4160-95ba-c96bc45a73d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3666), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3667), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73fd24a5-dddd-4837-8c9e-027cec3175f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3711), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3711), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74324406-77e4-4f78-b486-5ce18ecce5e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3783), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3784), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74836307-2d5d-46b6-a350-7fdf1a9bcee2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4695), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4695), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("754b4547-831d-48f5-a97a-efdd0b4cadc8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4479), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4479), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("766fde1a-e0f4-4a06-a565-749f0e62a512"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3929), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3930), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("771145fc-579e-4452-b331-11329dafab55"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3962), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3963), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("77517652-b41d-4954-bd95-6fa3554c03cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4682), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4683), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("778959a1-4baa-4f70-863f-92b4db0b1e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4072), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4073), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a3952d1-f745-4c48-ade4-a7a3483ac348"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4023), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4023), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a52a66a-759d-4e7b-b0f7-d5a48c5fcba7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4728), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4728), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7d064556-e6b3-410a-801e-8682b2bb75c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3793), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3794), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7fef5aaf-0232-4907-88ad-3d58e10c15d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4298), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4299), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("80d0ff9c-0d05-4727-a603-595dccd73f4b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4442), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4442), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81062b69-2d5e-4ff3-abc8-8b70fab4cc38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4011), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4012), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81423693-49b9-4554-a113-5db584f79a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4238), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4239), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8170d54f-b6c8-4bbc-9763-92b374544edd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4589), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4589), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81b1e3f7-525e-4b24-8e4b-1342909ef6cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3726), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3727), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8434011f-6ae2-419c-a77e-220dfb21c028"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4361), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4362), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("860c0937-8adf-4860-a7c7-5791aa8f7f19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4061), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4062), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8617a57c-d899-4320-b1e2-88c4b7704072"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4147), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4147), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("86668055-7b54-4cd3-aa7d-bfc0dc6b223e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4721), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4721), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("874eef90-39de-4367-a482-5f4d8c9d6f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4630), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4630), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("87af5a28-4975-4e91-8665-daabc185a403"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4170), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4171), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8bcd3442-31bd-40c6-a246-de59da0fe2ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4540), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4540), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c0b89fe-11fb-4196-a246-17a17f4592d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3917), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3917), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c5eb361-3fb3-478e-afa2-97970139dd69"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4202), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4203), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c9be888-fbcb-4394-bc9e-3e27571254fc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4755), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4755), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("924df98b-9eb0-4585-9376-87dbbda62e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3799), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3800), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("93111020-404c-4712-84cf-8febfda90bda"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4641), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4641), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9630a6a7-d51e-4912-82ec-3f9372dfbbd1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3992), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3992), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("969c634e-3f3a-419a-8526-e10edbd8c5ca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4403), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4403), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("97357fcd-5559-4a09-a7ac-901e3ef851bf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3642), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3643), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9836be8a-a6d8-40fa-968e-c228f89bb914"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4724), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4725), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("986291f1-06f6-40b0-81db-ca076898991c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4664), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4664), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("99016a1e-3bed-4399-bc4e-203e6fcd8a5a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4490), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4490), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9976bd33-0939-4751-8906-a4dfa5383329"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4085), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4085), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9b109e35-f3f4-4cc4-b260-531ae5baa6b9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4678), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4679), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9df91660-cf3c-41bf-bf99-2d4c95f1eb50"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4050), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4050), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9e620384-0990-4704-b3ee-1414ad4b5d8f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3686), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3687), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9eb802bb-91fd-4fbd-9d1b-e0a33f363e62"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3851), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3852), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a037fb68-185f-404c-a68d-b5191fcde40f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4399), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4399), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a0c8afb7-71e9-49ac-97e4-e30434d41bd4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3675), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3675), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a377d9c8-b36c-4398-b94d-dffd2b9b7e2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4120), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4120), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a4ecc6d6-aadd-4fdd-b2d8-211bffe8a60a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4552), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4552), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a6cb4b9e-865b-4a10-8b27-fc6e18053946"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3875), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3875), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7ced9df-e3f3-421c-b52e-e851561612cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3855), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3856), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7fc976d-e924-4b1a-80ba-25fa11c6eada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4555), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4556), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a8181d58-e6fb-4d84-9ba8-1d3426d63b25"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4486), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4486), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a9d7e3a9-1d94-48bf-a6b6-f2b4ea642c6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3861), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3862), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("abbd7ea3-5e9f-429b-9bc9-2eac05e365b0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3734), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3734), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ac4fd120-5184-43d2-bcbc-3aa661029deb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4506), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4506), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("acf5ce30-c463-45e1-9b36-0ae006ebdedf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4042), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4042), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ada316d2-d3bf-4f9e-a768-bc2485a08ada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4101), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4101), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b18fb01b-496b-4c19-b834-ccfb9247fb16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3900), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3900), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b1f86138-bd5e-4078-911b-4d81c78e4414"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4567), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4567), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b296bea3-8590-4c82-8060-87feed620d5f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4633), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4634), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b482e424-abfd-46ab-8919-015fe2cede4e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4736), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4737), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b5533594-0720-40fd-b3b2-52e3497b8b40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4769), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4770), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b586ac34-995f-4743-82f0-21f1f4e3d76d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3967), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3967), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b7c55d98-2ca5-498d-83c5-65821f69b0a9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4449), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4450), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b81b6fa4-1d67-4843-8001-d1236c0c046e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3843), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3843), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b99ec1be-734e-4f3f-b1d2-52dffc9ce612"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4315), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4316), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ba5c3b79-507e-4afd-bb65-641c9d859763"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3924), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3925), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bc79b37f-d82d-4f32-8e1f-9834734f114e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4592), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4593), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bce7924e-ffbb-4434-af88-e3b147b502a7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4611), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4612), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bd95782e-0020-4fa8-ac92-ff34a5438c2a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3879), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3880), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("becfbad6-9da2-48e7-8841-82609bb8cac9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4069), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4069), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bf4b78f7-863c-42d7-9465-fa11a6a9b175"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4732), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4732), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bfc76c18-4434-4aae-924a-d8dd126e4014"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4209), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4210), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c0cb789d-f745-4a0d-b166-763c499563c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4688), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4688), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c43440ae-99c5-4722-84f9-29dc752e81d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4498), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4498), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c5763f60-c5de-42d1-b97e-d78745e281f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4342), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4343), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c6c1fe44-2147-41ca-8100-48c1179545a0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3749), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3750), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8aad664-4329-40df-bdae-404d16ad28e0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4759), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4759), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8b369a5-b9c1-48f7-9aeb-e56e39eea7ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4615), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4616), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c95f54b9-743a-43c5-9336-753646b2bae0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4007), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4008), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cceb2b1b-d848-41a2-b987-1c256c1caf3d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4415), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4415), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cede99f7-0fac-416c-aca0-359b1ceb6665"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4186), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4186), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf34f086-c4f2-49c2-9b12-8cec89f76d73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4660), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4660), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf86d807-0488-4fdc-86cd-fbc1849c2232"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4668), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4668), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cfc21067-9cb1-4e8f-bd06-698befaad209"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3630), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d019d451-4ef0-4a26-ac7c-3fe0040a03f8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3995), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3996), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d03dff77-9c74-49bb-a5a7-10f00a52eadf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4182), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4183), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d04b3225-4455-4dbf-879c-52521881ba32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4139), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4139), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d0eb085d-9415-4f52-aee2-3d2631ee17e2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4054), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4054), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d10f35d9-7528-41ed-8362-25a13e3d19c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4494), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4495), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d1569f7a-17b0-4499-86bc-8da3f62d78df"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4528), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4529), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d21fb3f4-063a-464c-8732-c5a8aeca001d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4206), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4206), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d28d60b7-a11f-420d-9b78-8978221a12b7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4699), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4700), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d3c64bb3-2e70-4eac-b1fb-5bf4fdc374c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3625), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3626), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d472d4c8-b215-4477-8f0d-687e8f698158"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4574), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4575), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d48e3d13-de43-429a-bbc3-0cdcc1ca8ea1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3911), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3912), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d502b3f2-936d-449b-90cd-a783fb2942f4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3896), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3897), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d57947e1-16c0-4b9d-9573-3d2767815451"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3988), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3988), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d59b6e17-2905-46f7-977b-545cdbf9251e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3770), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3771), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d5b77b5b-ace9-4c54-af3e-bae4449696c8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3693), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3693), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7919a88-1042-4951-991d-be49aba34367"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4031), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4031), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7ab6445-47e1-4602-8c29-66d500b5ca51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4517), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4517), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d927c02d-5ebc-4b7b-9b7f-f7fab5ca3bc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4548), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4548), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d93ff951-cc6d-4c57-addf-cf34965074c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4038), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4039), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d98bab5e-1b3d-4138-b97c-b71db86d30c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3946), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3946), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("dbe48e80-b8d1-4140-b030-8a1c348ac3c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3934), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3934), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("de36b74c-6770-4a9a-8e53-3a032d3170af"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4671), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4672), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("deed3198-0bfa-4313-9919-64090878ff40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4323), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4323), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0b42e50-0db1-440a-b392-7056af38f492"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4142), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4143), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0f45b74-6c7a-4c6f-b913-41d887443cd3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3745), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3746), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e1a611db-e710-467c-9404-de8e82dfcdc7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3697), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3697), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e4866ef8-7a9f-49ea-af3d-2f2adc388255"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3980), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3980), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e49dcd0d-3820-4c45-9990-3b799d0b733b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4295), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4295), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e5be72d6-38eb-44c4-b087-6e1bc0120733"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3999), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4000), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e6056baa-29fe-43b4-bc21-e0c8a89ec112"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4797), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4797), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7274bb8-ab3c-4138-b091-9ddddef4499e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4162), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4163), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7300f3a-c370-43d5-b268-44286a302c60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4426), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4427), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e73c2a6a-ca1e-4149-b692-e8fb3cc67d48"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3905), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3906), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7fd9c3b-207a-426d-829c-5864087b0ceb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4434), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4434), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e88eef15-2e0c-42c2-a5f5-4cba09df8b7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4766), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4766), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e964019e-39bf-4477-ba06-9cff9d3e44ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4710), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4710), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eae2509e-0b08-42c6-9123-205c725291a2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4751), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4752), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb447e65-d19d-491e-b4bf-a0517a64b79b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3646), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3646), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb5281e2-efc5-444c-a92d-1a2603721b4f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4283), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4283), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb7554df-f80e-4194-886d-bee5babdd62c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3659), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3659), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eba41453-c8c2-491d-bb86-9598de7ffab2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3818), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3818), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ec289217-c6b7-4837-8b8a-2cbc9fbc5df0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3889), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3890), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ed375f05-4451-42fa-bebe-96c194fcfddf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4532), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4533), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ee736134-e8a9-4f6b-9057-aefe27a222d2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4089), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4089), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ef4b1a02-bf0c-4aee-bf72-0fb21a303bc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4279), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4279), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f30ae7c6-9f2d-42d9-9ba4-9a44a658fb6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4254), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4254), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f318741a-31bf-41ad-9e30-627955fbcefe"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4446), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4446), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48d909e-a7ac-4990-b8da-2bc87429e7a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4291), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4291), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48df6b4-ada9-4504-9a4a-ef68a0edaf8d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4350), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4350), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f4f8d8c5-735c-42f3-b534-bf92030606d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4034), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4035), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f5c0457d-d3a6-4b87-931a-0ccdc4bdb343"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4626), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4627), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f715b6bb-91a0-452a-a589-0c4500aa3a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3719), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3719), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f7c81ff9-6224-42ea-8080-eed6479b334a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4174), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4174), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8924f7d-95dc-4e9f-b092-d0ad5046c1a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4608), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4608), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8d3db4c-f63f-4c2a-84bf-174411fe1e22"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4217), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4217), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("fb03709a-af48-452d-b711-d5990db0fa38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3949), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3950), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ff23c77a-22fa-4e27-86f6-a71cb4e43e36"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4331), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(4331), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3555), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3556), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3496), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3496), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3553), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3553), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3526), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3526), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3547), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3548), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3550), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3550), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3522), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3523), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3520), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3520), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3561), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3562), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3511), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3512), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3504), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3505), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3499), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3499), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3514), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3514), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3508), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3509), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3564), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3564), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3516), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3517), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3558), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3558), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3566), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3567), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3502), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3502), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("e44e12a4-df37-401e-afc9-08024be3991a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3492), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3492), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3378), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 16, 11, 34, 39, 766, DateTimeKind.Unspecified).AddTicks(3387), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
