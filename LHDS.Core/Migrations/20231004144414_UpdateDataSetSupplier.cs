using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataSetSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecryptionManualTriggerUrl",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "LandingManualTriggerUrl",
                schema: "Configuration",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "DataSetSupplier",
                schema: "Configuration",
                table: "DataSets")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(162), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(163), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(129), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(130), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016b7c2a-3cb9-4433-a79e-b55ca988eb57"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(376), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(376), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016f00d9-f56e-4610-a38d-a3c49023d4c1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1133), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1134), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("01abe9ac-bbd0-41e7-a8c3-b7c1a967cd1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(856), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(856), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("024eb41e-404f-461f-ad10-fcb9a6c37fe4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1527), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1527), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("02fdb2c0-8fb5-4a4a-b1e0-68614da128e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1201), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1201), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("04c59f48-ba31-4809-94bb-f4f5e1cf4460"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(696), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(696), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("053eade2-698d-4cbd-b84b-213adb1ea3f5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1184), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1185), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("05803762-b255-48b9-bdae-802e7a6aaf05"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1439), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1440), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("07c3cca3-7eb1-451e-8087-ee1f2aa733ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(428), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(428), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("08ae5632-37ad-4f2a-bfd2-a3ef3c73d6cb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(679), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(680), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b1725d-18d8-4222-8c43-a4bea3ba8b06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(683), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(684), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b6077f-74a6-4cc0-9992-0c7723d81416"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(910), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(910), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09f8009d-8453-4ce5-b7b4-8ed6eb4b2d31"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(931), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(931), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09fb5c58-0336-4b20-b521-83e5cd8b6b49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1679), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1679), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a5fc198-19b8-43ae-b798-45506c385e20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1197), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1197), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a946e6b-f495-4cd4-8639-bc17173d1864"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(783), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(784), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ad1dd7b-4d2a-417e-a6b2-6135af7c19b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1361), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1362), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bbbd975-da17-4340-95ae-34646282eba2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1414), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1415), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f1e1-3583-4ae2-8eac-6df87422eb79"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(527), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(527), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ca7ae3a-036d-49a4-bb59-4cf3b5435ce3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1108), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1109), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0cfc4aff-ce97-4c48-8258-0ba55e63c49c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1080), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1080), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0e166c8f-eb6c-43d0-b12c-f49d44953e6a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1213), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1214), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ef487f9-476e-4ec8-899d-541478430e16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1125), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1126), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f37bbe4-afbf-474b-815e-f0dbdb653540"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1394), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1395), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f5f262d-55d1-4611-9b1f-355ec4aab0d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1250), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1251), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0fe408cb-5610-461b-8260-510814bc54bb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(816), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(816), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1091b3d3-a4a8-44b8-b4e5-c10c1014dc3f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1300), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1300), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1481245d-c122-4890-b5d4-ae87ff63c62a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1683), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1683), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15096c24-3ea9-4f19-a7a0-ee4c5a09dd12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1494), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1495), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15422b24-e865-4db5-8536-3cedb69e3efc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(481), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(481), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15d5d359-d192-43ff-b7bd-b4ad0406146e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1146), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1146), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("16143ebe-976a-4a9e-805d-8f85265e1003"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1600), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1600), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("17b7fdc6-616c-4fa7-8ee6-f970443d0c11"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1331), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1332), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("18d0f511-1978-46cc-9721-0475433bccc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1076), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1076), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1b3388cf-2e88-431e-815f-b8eef77568f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(554), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(555), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c34861b-aec0-4508-99a9-c4f79a76abfc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(872), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(872), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c71fe1b-41f4-4e7c-aa5f-c1ecbcc7455a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1304), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1304), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d11df66-c4a7-4e85-84bb-77a5fe923590"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1531), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1531), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d661e5a-fcf8-4dec-8634-07f5f6c3e741"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(455), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(455), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1e11b982-faad-4e5c-a8bd-95f1d4b5c648"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(775), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(776), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1f381e8f-19e4-4685-a84b-6c7f41ca0480"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1369), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1369), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("223b3cc1-56f6-44ed-89cf-b13b06e49e99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1296), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1296), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("233b0b2f-4620-4adb-8107-8a7e3fea5b1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(538), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(539), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("23d3274a-f5e4-47a2-8ccb-a4d5d5aa84b2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(840), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(840), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("25cb156f-86d0-4c68-9583-6ddbe08042cf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(349), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(349), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("261902dc-c53e-4017-b0ab-7e350ce62c7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1287), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1288), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("26331b24-6f9f-4f6f-bfcb-c8ae81dca9a8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1523), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1523), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2655335d-4a67-4be7-a8f1-860a7c7aec20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1205), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1205), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28246209-2a86-456c-97a5-00eb5ed89276"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1071), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1072), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28ce4c02-8c50-465c-b2e9-a6065eb5d522"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1291), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1292), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2afb470a-7b78-4a3f-afab-fc817dcae6e8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1703), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1704), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b4b318a-f79f-4eb6-9c8d-90bbc1a5baa2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(978), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(979), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b762785-2318-4407-b52b-c93142fc067f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(562), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(563), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2c1c993a-72f3-4bf3-ad8e-cbfcc0cdd323"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1460), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1460), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2cb2c37e-677c-4918-93b9-bdb4a18fdc99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(864), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(864), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2dc110e6-417f-41d1-97aa-e1d8bcae2126"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(394), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(394), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2de218bf-f65c-44c2-8b97-e813931aa669"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(424), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(424), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2e7f0550-7ddd-4035-899f-b5b0c01a2f43"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1687), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1688), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2f2df5d0-141e-4cf8-83a3-13c520078e51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2fe17d9f-3ff3-41a9-a75f-f9776e8278f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(622), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(622), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("310cdb68-574f-4904-ac15-589db18d93a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1266), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1267), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("31b8a2cb-7ea8-4212-ba08-93f07f52a926"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(934), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(935), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3277f9a2-be19-44f7-a320-4e57b08186c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(502), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(502), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("32f7d725-972f-4c61-9cd7-120c4575f6da"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(463), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(464), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3457368f-3486-4598-b75e-6c4862acc273"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1674), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1674), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3493ddcc-e3db-4ab5-bc2c-bd0c53910fd9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1612), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1613), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3504618b-fbdf-4d1a-8ce3-4beaeb9f36b1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1035), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1036), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("352d7e99-e0a3-417a-a9f4-41764c8037d3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1662), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1662), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("354a00f1-919f-46e5-bc27-558864dd6282"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(514), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(515), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("35e8f22d-40e3-454f-8c82-af50e81227e5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(367), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(367), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("37ca1513-b100-4af7-a7dc-5535c9041b60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1423), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1423), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("39609eae-9ca8-4a8d-93a5-52d28889e80b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1117), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1117), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a41011e-8561-47f7-9efd-f98a2b121c01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1498), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1499), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a6933ca-e959-4cb9-a5c7-7c85776cd2c3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1588), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1588), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3c38425d-ca08-458e-8aa6-6dbde22d075f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1431), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1432), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3d1c757f-4c5c-41c4-9431-3d76f54405fb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(332), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(333), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f07700f-b59e-4e13-b8a6-e04872461ba3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(997), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(997), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f51ce5d-7894-4332-9ce2-28f9bfffd9cc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1023), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1024), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3fbc97b7-a830-4e59-a534-18e27c1659de"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1217), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1218), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("405afb0c-1252-4c8e-b1d7-193dc2ea61ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(634), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(635), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("40a02930-815a-4396-9893-7138128667d0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1047), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1048), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("424e2ade-0b15-4014-bd24-0e66e50c69a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(700), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(700), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("42c1ad57-365d-43c8-a746-503c9b723060"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(506), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(506), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("43d87e04-fddf-406d-9172-adffc4295be0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1142), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1142), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4737f612-058e-4491-829f-a336fb9136e1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(836), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(836), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("479b24dc-8673-4a71-ac3b-2b45d1669614"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1193), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1193), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("47c4a842-a333-4034-9741-a8aea34e6159"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(546), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(547), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("48e9e1a9-9233-495e-a5c5-c1398a433ad8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1456), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1456), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a71dd62-bef0-4d7b-a967-4ae93751a69b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(497), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(498), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a79190f-0271-4e66-936f-8d4bfd10a5ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(353), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(354), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4affbb36-dbe5-483b-bdb9-3b21565e0658"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(724), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(724), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4baf1d59-6c43-4d72-a1f9-318e44cf1bae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(914), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(915), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c55b3a2-b325-4835-939e-5e3ef835bf66"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(824), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(824), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c80e258-93f8-4631-b83b-d5a1254bbdca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(875), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(876), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4cdebe09-d9ab-4229-91e2-4ca9e6899089"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1039), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1040), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4d556e6a-4037-4c29-a40d-3fe284679693"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(664), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(664), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4e3e8c9e-58ed-4aab-acf8-b969e8d44cd5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(558), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(559), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb23b6a-a682-482d-b9c3-689f29d1856a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1283), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1284), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb79c3b-df7a-4c59-9d97-ac930ca623e6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1209), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1209), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4ffb768c-34f2-4aea-ab97-497a3d5c9e32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1258), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1259), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50557d75-1849-4528-8b57-68a0eebb4877"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(969), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(970), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50c22935-6a40-4b97-b7d5-e7acd32f7e19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(403), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(403), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("52f25a39-d50f-4e0e-9dd5-729c7f9c3fee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(764), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(764), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("53dbbec5-311a-49f3-9dc7-143b9f150eab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1641), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1641), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("54419442-e140-4642-ab30-af6e2dd74228"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(852), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(852), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("546f7053-f243-43f2-8479-c97165c1c97c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1691), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1692), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("556326f1-54e5-4c38-96ff-2a8782d03f59"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1410), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1411), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("559aef93-6209-456a-8b5c-d859e8c00e49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1695), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1695), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56f2ea58-47f0-4229-8c22-b101d390bd7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1064), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1064), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56faac0a-ccd0-4019-9581-108006412c95"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(955), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(956), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5712c2a5-7e44-4c16-8e98-1d647331f00d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(472), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(473), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("576c97af-fde1-408d-9042-b75448adc98d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1608), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1608), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("578e46f1-fb0b-4766-adb1-3204d0ee92c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1027), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1028), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5870d3d6-c36d-444a-bd48-0ee20b73c8dc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1233), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1234), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ae2fff6-23ec-46cc-a10e-7dd3d0c5cd1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1435), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5b3ef045-3abc-48e2-8198-c31018c99481"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(385), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(386), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ddcf642-c337-471b-bccf-d7feebbd417e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(570), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(570), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5dfa3bcf-5f87-4ae7-82af-f4467905e4c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(407), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(407), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("61e24c11-d0d9-4e4c-9ba8-f44aab8bd573"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(618), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(618), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("62d21d9d-878c-4d7f-a814-feca5a481cee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1067), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1068), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6307fe1e-287a-4e0d-b949-eb8696364f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(905), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(906), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6312a346-339e-43f1-bf4e-ab46a2669f80"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(550), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(551), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("635198f6-16c4-4033-a433-58cf962ffa6b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(519), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(519), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63c6725f-d1d1-4349-8320-275874d68594"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1092), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1092), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ce742c-4532-4211-9f8d-32ac44e1d557"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1031), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1032), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ef80ea-feb3-46e3-ae65-493ae81df639"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(493), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(493), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6416ef97-03b3-4a28-9fcf-195b92d5dc6e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1238), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1238), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("647e5d2d-589a-4caf-b777-2d3a8122cd2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(716), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(717), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("64e88c5b-31a0-4857-b96e-342b71572897"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1353), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1353), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("665021ae-e50e-4874-a502-7189380d5f53"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1221), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1222), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6694a4bb-7729-43d0-8b66-e904d79cc2b3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1154), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1154), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("674ced77-2991-46c0-8e65-abe2fe966c03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1451), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1452), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6828c5b8-50fe-41a2-8b69-6eaa22fd86f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1373), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1373), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("689f2c1c-6a26-4daa-aff2-2d223def8b52"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(712), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(712), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6a4af2a2-ed38-4e42-9e98-1d630f8c396d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1645), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1645), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6aacbf40-a3cc-4993-bb1a-3c92aa56366d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1551), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6bc62b7a-c062-48a1-846a-127626b977f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1180), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1181), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6d5c31f4-56f5-4cf3-a875-ad137c8293d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1386), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1386), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6e37a8c1-9a0d-4cce-a268-724bd1cd3d17"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1051), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1052), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f328079-ecd0-4b2e-8b37-e4f40f228f9a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(901), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(902), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f7779fe-1ecc-4da1-8647-51226936afc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1112), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1113), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6fac827e-b9bd-4ca2-90c4-6ec500a05e03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(973), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(974), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70de2594-ecea-41ea-8fac-b0302c2b5e65"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1060), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1060), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70eb906b-851b-4278-9540-2badcc279e10"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1246), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1246), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7168fb0c-1142-4924-b27d-d39831a75037"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(943), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(943), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("716fab26-5b55-4d46-a4de-10873d0625b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(803), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(804), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("71c34eec-9b5f-49c7-a3be-ace36b45f15a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1518), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1519), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("72b1c7a9-b6d5-40a7-a699-bf6598047882"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(868), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(868), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73b108e5-328c-4160-95ba-c96bc45a73d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(389), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(390), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73fd24a5-dddd-4837-8c9e-027cec3175f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(432), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(432), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74324406-77e4-4f78-b486-5ce18ecce5e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(523), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(523), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74836307-2d5d-46b6-a350-7fdf1a9bcee2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1592), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1592), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("754b4547-831d-48f5-a97a-efdd0b4cadc8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1308), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1309), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("766fde1a-e0f4-4a06-a565-749f0e62a512"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(671), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(672), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("771145fc-579e-4452-b331-11329dafab55"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(704), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(704), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("77517652-b41d-4954-bd95-6fa3554c03cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1560), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1560), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("778959a1-4baa-4f70-863f-92b4db0b1e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(832), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(832), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a3952d1-f745-4c48-ade4-a7a3483ac348"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(780), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a52a66a-759d-4e7b-b0f7-d5a48c5fcba7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1624), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7d064556-e6b3-410a-801e-8682b2bb75c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(530), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(531), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7fef5aaf-0232-4907-88ad-3d58e10c15d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1104), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1105), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("80d0ff9c-0d05-4727-a603-595dccd73f4b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1271), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1271), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81062b69-2d5e-4ff3-abc8-8b70fab4cc38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(772), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(772), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81423693-49b9-4554-a113-5db584f79a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1043), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1044), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8170d54f-b6c8-4bbc-9763-92b374544edd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1443), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1444), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81b1e3f7-525e-4b24-8e4b-1342909ef6cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(467), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(468), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8434011f-6ae2-419c-a77e-220dfb21c028"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1188), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1189), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("860c0937-8adf-4860-a7c7-5791aa8f7f19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(819), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(820), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8617a57c-d899-4320-b1e2-88c4b7704072"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(926), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(927), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("86668055-7b54-4cd3-aa7d-bfc0dc6b223e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1616), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1617), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("874eef90-39de-4367-a482-5f4d8c9d6f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1506), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1507), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("87af5a28-4975-4e91-8665-daabc185a403"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(947), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(948), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8bcd3442-31bd-40c6-a246-de59da0fe2ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1390), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1390), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c0b89fe-11fb-4196-a246-17a17f4592d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(658), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(659), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c5eb361-3fb3-478e-afa2-97970139dd69"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(984), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(985), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c9be888-fbcb-4394-bc9e-3e27571254fc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1653), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1653), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("924df98b-9eb0-4585-9376-87dbbda62e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(534), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(535), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("93111020-404c-4712-84cf-8febfda90bda"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1514), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1515), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9630a6a7-d51e-4912-82ec-3f9372dfbbd1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(751), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(752), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("969c634e-3f3a-419a-8526-e10edbd8c5ca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1229), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1230), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("97357fcd-5559-4a09-a7ac-901e3ef851bf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(358), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(359), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9836be8a-a6d8-40fa-968e-c228f89bb914"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1620), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1621), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("986291f1-06f6-40b0-81db-ca076898991c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1539), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1540), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("99016a1e-3bed-4399-bc4e-203e6fcd8a5a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1340), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1341), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9976bd33-0939-4751-8906-a4dfa5383329"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(844), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(844), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9b109e35-f3f4-4cc4-b260-531ae5baa6b9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1556), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1556), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9df91660-cf3c-41bf-bf99-2d4c95f1eb50"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(807), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(808), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9e620384-0990-4704-b3ee-1414ad4b5d8f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(411), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(412), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9eb802bb-91fd-4fbd-9d1b-e0a33f363e62"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(574), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(574), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a037fb68-185f-404c-a68d-b5191fcde40f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1225), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1226), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a0c8afb7-71e9-49ac-97e4-e30434d41bd4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(398), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(399), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a377d9c8-b36c-4398-b94d-dffd2b9b7e2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(880), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(880), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a4ecc6d6-aadd-4fdd-b2d8-211bffe8a60a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1402), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1403), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a6cb4b9e-865b-4a10-8b27-fc6e18053946"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(626), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(626), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7ced9df-e3f3-421c-b52e-e851561612cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(578), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(578), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7fc976d-e924-4b1a-80ba-25fa11c6eada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1406), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1407), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a8181d58-e6fb-4d84-9ba8-1d3426d63b25"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1336), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1336), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a9d7e3a9-1d94-48bf-a6b6-f2b4ea642c6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(613), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(614), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("abbd7ea3-5e9f-429b-9bc9-2eac05e365b0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(476), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(477), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ac4fd120-5184-43d2-bcbc-3aa661029deb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1357), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1358), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("acf5ce30-c463-45e1-9b36-0ae006ebdedf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(799), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(800), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ada316d2-d3bf-4f9e-a768-bc2485a08ada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(860), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(860), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b18fb01b-496b-4c19-b834-ccfb9247fb16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(646), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(647), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b1f86138-bd5e-4078-911b-4d81c78e4414"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1419), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b296bea3-8590-4c82-8060-87feed620d5f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1510), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1511), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b482e424-abfd-46ab-8919-015fe2cede4e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1633), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b5533594-0720-40fd-b3b2-52e3497b8b40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1670), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1670), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b586ac34-995f-4743-82f0-21f1f4e3d76d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(708), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(708), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b7c55d98-2ca5-498d-83c5-65821f69b0a9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1279), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1279), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b81b6fa4-1d67-4843-8001-d1236c0c046e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(566), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(567), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b99ec1be-734e-4f3f-b1d2-52dffc9ce612"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1121), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1121), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ba5c3b79-507e-4afd-bb65-641c9d859763"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(668), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(668), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bc79b37f-d82d-4f32-8e1f-9834734f114e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1448), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1448), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bce7924e-ffbb-4434-af88-e3b147b502a7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1486), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1487), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bd95782e-0020-4fa8-ac92-ff34a5438c2a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(630), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(631), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("becfbad6-9da2-48e7-8841-82609bb8cac9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(828), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(828), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bf4b78f7-863c-42d7-9465-fa11a6a9b175"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1628), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1629), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bfc76c18-4434-4aae-924a-d8dd126e4014"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(992), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(993), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c0cb789d-f745-4a0d-b166-763c499563c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1583), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1584), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c43440ae-99c5-4722-84f9-29dc752e81d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1349), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1349), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c5763f60-c5de-42d1-b97e-d78745e281f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1150), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1150), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c6c1fe44-2147-41ca-8100-48c1179545a0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(489), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(489), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8aad664-4329-40df-bdae-404d16ad28e0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1657), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1658), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8b369a5-b9c1-48f7-9aeb-e56e39eea7ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1490), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1491), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c95f54b9-743a-43c5-9336-753646b2bae0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(768), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(768), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cceb2b1b-d848-41a2-b987-1c256c1caf3d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1242), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1242), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cede99f7-0fac-416c-aca0-359b1ceb6665"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(965), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(965), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf34f086-c4f2-49c2-9b12-8cec89f76d73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1535), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1536), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf86d807-0488-4fdc-86cd-fbc1849c2232"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1543), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1544), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cfc21067-9cb1-4e8f-bd06-698befaad209"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(345), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(345), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d019d451-4ef0-4a26-ac7c-3fe0040a03f8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(756), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(756), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d03dff77-9c74-49bb-a5a7-10f00a52eadf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(961), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(962), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d04b3225-4455-4dbf-879c-52521881ba32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(918), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(919), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d0eb085d-9415-4f52-aee2-3d2631ee17e2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(812), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(812), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d10f35d9-7528-41ed-8362-25a13e3d19c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1345), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1345), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d1569f7a-17b0-4499-86bc-8da3f62d78df"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1378), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1378), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d21fb3f4-063a-464c-8732-c5a8aeca001d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(988), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(989), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d28d60b7-a11f-420d-9b78-8978221a12b7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1596), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1596), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d3c64bb3-2e70-4eac-b1fb-5bf4fdc374c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(339), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(340), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d472d4c8-b215-4477-8f0d-687e8f698158"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1427), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1428), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d48e3d13-de43-429a-bbc3-0cdcc1ca8ea1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(654), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(655), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d502b3f2-936d-449b-90cd-a783fb2942f4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(642), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(642), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d57947e1-16c0-4b9d-9573-3d2767815451"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(728), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(729), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d59b6e17-2905-46f7-977b-545cdbf9251e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(510), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(511), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d5b77b5b-ace9-4c54-af3e-bae4449696c8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(416), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(416), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7919a88-1042-4951-991d-be49aba34367"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(787), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(788), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7ab6445-47e1-4602-8c29-66d500b5ca51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1365), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1366), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d927c02d-5ebc-4b7b-9b7f-f7fab5ca3bc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1398), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1399), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d93ff951-cc6d-4c57-addf-cf34965074c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(795), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(796), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d98bab5e-1b3d-4138-b97c-b71db86d30c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(688), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(688), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("dbe48e80-b8d1-4140-b030-8a1c348ac3c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(675), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(676), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("de36b74c-6770-4a9a-8e53-3a032d3170af"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("deed3198-0bfa-4313-9919-64090878ff40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1129), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1130), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0b42e50-0db1-440a-b392-7056af38f492"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(922), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(923), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0f45b74-6c7a-4c6f-b913-41d887443cd3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(485), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(485), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e1a611db-e710-467c-9404-de8e82dfcdc7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(419), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(420), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e4866ef8-7a9f-49ea-af3d-2f2adc388255"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(720), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(721), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e49dcd0d-3820-4c45-9990-3b799d0b733b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1100), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1101), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e5be72d6-38eb-44c4-b087-6e1bc0120733"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(760), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(760), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e6056baa-29fe-43b4-bc21-e0c8a89ec112"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1699), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1699), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7274bb8-ab3c-4138-b091-9ddddef4499e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(939), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(939), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7300f3a-c370-43d5-b268-44286a302c60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1254), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1255), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e73c2a6a-ca1e-4149-b692-e8fb3cc67d48"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(650), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(651), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7fd9c3b-207a-426d-829c-5864087b0ceb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1262), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1263), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e88eef15-2e0c-42c2-a5f5-4cba09df8b7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1666), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1667), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e964019e-39bf-4477-ba06-9cff9d3e44ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1604), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eae2509e-0b08-42c6-9123-205c725291a2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1649), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1649), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb447e65-d19d-491e-b4bf-a0517a64b79b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(363), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(363), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb5281e2-efc5-444c-a92d-1a2603721b4f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1088), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1088), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb7554df-f80e-4194-886d-bee5babdd62c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(381), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(382), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eba41453-c8c2-491d-bb86-9598de7ffab2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(542), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(543), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ec289217-c6b7-4837-8b8a-2cbc9fbc5df0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(638), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(639), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ed375f05-4451-42fa-bebe-96c194fcfddf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1382), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1382), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ee736134-e8a9-4f6b-9057-aefe27a222d2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(848), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(848), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ef4b1a02-bf0c-4aee-bf72-0fb21a303bc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f30ae7c6-9f2d-42d9-9ba4-9a44a658fb6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1056), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1056), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f318741a-31bf-41ad-9e30-627955fbcefe"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1275), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1275), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48d909e-a7ac-4990-b8da-2bc87429e7a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1096), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1097), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48df6b4-ada9-4504-9a4a-ef68a0edaf8d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1176), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1176), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f4f8d8c5-735c-42f3-b534-bf92030606d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(791), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(792), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f5c0457d-d3a6-4b87-931a-0ccdc4bdb343"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1502), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1503), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f715b6bb-91a0-452a-a589-0c4500aa3a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(459), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(459), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f7c81ff9-6224-42ea-8080-eed6479b334a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(951), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(952), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8924f7d-95dc-4e9f-b092-d0ad5046c1a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1464), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1464), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8d3db4c-f63f-4c2a-84bf-174411fe1e22"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1001), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1002), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("fb03709a-af48-452d-b711-d5990db0fa38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(692), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(692), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ff23c77a-22fa-4e27-86f6-a71cb4e43e36"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1138), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1138), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(260), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(261), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(193), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(194), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(233), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(234), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(225), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(226), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(228), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(229), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(231), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(231), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(223), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(223), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(220), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(220), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(267), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(267), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(211), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(212), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(204), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(204), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(198), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(198), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(214), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(214), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(208), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(209), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(271), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(271), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(216), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(217), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(263), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(264), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(276), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(201), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(201), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("e44e12a4-df37-401e-afc9-08024be3991a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(187), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(188), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 146, DateTimeKind.Unspecified).AddTicks(9989), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 4, 14, 44, 14, 147, DateTimeKind.Unspecified).AddTicks(1), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_SupplierId",
                schema: "Configuration",
                table: "DataSets",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSets_Suppliers_SupplierId",
                schema: "Configuration",
                table: "DataSets",
                column: "SupplierId",
                principalSchema: "Configuration",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSets_Suppliers_SupplierId",
                schema: "Configuration",
                table: "DataSets");

            migrationBuilder.DropIndex(
                name: "IX_DataSets_SupplierId",
                schema: "Configuration",
                table: "DataSets");

            migrationBuilder.AddColumn<string>(
                name: "DecryptionManualTriggerUrl",
                schema: "Configuration",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandingManualTriggerUrl",
                schema: "Configuration",
                table: "Suppliers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DataSetSupplier",
                schema: "Configuration",
                table: "DataSets",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1084), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "CreatedDate", "DataSetSupplier", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1066), new TimeSpan(0, 0, 0, 0, 0)), "EMIS", new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1067), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016b7c2a-3cb9-4433-a79e-b55ca988eb57"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1242), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1242), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("016f00d9-f56e-4610-a38d-a3c49023d4c1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1864), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1864), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("01abe9ac-bbd0-41e7-a8c3-b7c1a967cd1a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1636), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1637), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("024eb41e-404f-461f-ad10-fcb9a6c37fe4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2218), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2218), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("02fdb2c0-8fb5-4a4a-b1e0-68614da128e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1916), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1916), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("04c59f48-ba31-4809-94bb-f4f5e1cf4460"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1499), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1501), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("053eade2-698d-4cbd-b84b-213adb1ea3f5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1898), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1899), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("05803762-b255-48b9-bdae-802e7a6aaf05"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2148), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2148), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("07c3cca3-7eb1-451e-8087-ee1f2aa733ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1295), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1296), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("08ae5632-37ad-4f2a-bfd2-a3ef3c73d6cb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1485), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1486), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b1725d-18d8-4222-8c43-a4bea3ba8b06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1489), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1489), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09b6077f-74a6-4cc0-9992-0c7723d81416"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1673), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1673), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09f8009d-8453-4ce5-b7b4-8ed6eb4b2d31"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1692), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1693), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("09fb5c58-0336-4b20-b521-83e5cd8b6b49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2343), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2343), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a5fc198-19b8-43ae-b798-45506c385e20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1911), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1912), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0a946e6b-f495-4cd4-8639-bc17173d1864"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1567), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1568), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ad1dd7b-4d2a-417e-a6b2-6135af7c19b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2071), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2072), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bbbd975-da17-4340-95ae-34646282eba2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2125), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2125), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0bc8f1e1-3583-4ae2-8eac-6df87422eb79"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1369), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1370), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ca7ae3a-036d-49a4-bb59-4cf3b5435ce3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1842), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1842), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0cfc4aff-ce97-4c48-8258-0ba55e63c49c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1812), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1813), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0e166c8f-eb6c-43d0-b12c-f49d44953e6a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1932), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1933), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0ef487f9-476e-4ec8-899d-541478430e16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1856), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1856), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f37bbe4-afbf-474b-815e-f0dbdb653540"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2106), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2106), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0f5f262d-55d1-4611-9b1f-355ec4aab0d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1973), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1974), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("0fe408cb-5610-461b-8260-510814bc54bb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1598), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1599), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1091b3d3-a4a8-44b8-b4e5-c10c1014dc3f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2029), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2030), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1481245d-c122-4890-b5d4-ae87ff63c62a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2346), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2347), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15096c24-3ea9-4f19-a7a0-ee4c5a09dd12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2188), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2188), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15422b24-e865-4db5-8536-3cedb69e3efc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1324), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1325), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("15d5d359-d192-43ff-b7bd-b4ad0406146e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1879), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1880), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("16143ebe-976a-4a9e-805d-8f85265e1003"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2273), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2274), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("17b7fdc6-616c-4fa7-8ee6-f970443d0c11"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2044), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2045), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("18d0f511-1978-46cc-9721-0475433bccc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1809), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1809), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1b3388cf-2e88-431e-815f-b8eef77568f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1395), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1396), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c34861b-aec0-4508-99a9-c4f79a76abfc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1654), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1655), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1c71fe1b-41f4-4e7c-aa5f-c1ecbcc7455a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2033), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2034), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d11df66-c4a7-4e85-84bb-77a5fe923590"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2222), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2222), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1d661e5a-fcf8-4dec-8634-07f5f6c3e741"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1302), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1303), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1e11b982-faad-4e5c-a8bd-95f1d4b5c648"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1559), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1560), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("1f381e8f-19e4-4685-a84b-6c7f41ca0480"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2081), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2081), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("223b3cc1-56f6-44ed-89cf-b13b06e49e99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2026), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2026), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("233b0b2f-4620-4adb-8107-8a7e3fea5b1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1380), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1381), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("23d3274a-f5e4-47a2-8ccb-a4d5d5aa84b2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1621), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1621), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("25cb156f-86d0-4c68-9583-6ddbe08042cf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1219), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1220), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("261902dc-c53e-4017-b0ab-7e350ce62c7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2014), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2014), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("26331b24-6f9f-4f6f-bfcb-c8ae81dca9a8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2214), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2215), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2655335d-4a67-4be7-a8f1-860a7c7aec20"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1920), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1921), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28246209-2a86-456c-97a5-00eb5ed89276"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1804), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1804), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("28ce4c02-8c50-465c-b2e9-a6065eb5d522"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2022), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2023), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2afb470a-7b78-4a3f-afab-fc817dcae6e8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2370), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2370), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b4b318a-f79f-4eb6-9c8d-90bbc1a5baa2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1733), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1734), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2b762785-2318-4407-b52b-c93142fc067f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1407), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1407), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2c1c993a-72f3-4bf3-ad8e-cbfcc0cdd323"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2173), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2173), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2cb2c37e-677c-4918-93b9-bdb4a18fdc99"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1647), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1648), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2dc110e6-417f-41d1-97aa-e1d8bcae2126"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1258), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1258), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2de218bf-f65c-44c2-8b97-e813931aa669"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1292), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1292), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2e7f0550-7ddd-4035-899f-b5b0c01a2f43"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2356), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2356), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2f2df5d0-141e-4cf8-83a3-13c520078e51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2307), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2307), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("2fe17d9f-3ff3-41a9-a75f-f9776e8278f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1432), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1432), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("310cdb68-574f-4904-ac15-589db18d93a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1993), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1993), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("31b8a2cb-7ea8-4212-ba08-93f07f52a926"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1696), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1696), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3277f9a2-be19-44f7-a320-4e57b08186c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1345), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1345), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("32f7d725-972f-4c61-9cd7-120c4575f6da"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1310), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1310), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3457368f-3486-4598-b75e-6c4862acc273"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2339), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2340), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3493ddcc-e3db-4ab5-bc2c-bd0c53910fd9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2284), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2285), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3504618b-fbdf-4d1a-8ce3-4beaeb9f36b1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1771), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1771), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("352d7e99-e0a3-417a-a9f4-41764c8037d3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2328), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2329), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("354a00f1-919f-46e5-bc27-558864dd6282"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1358), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1359), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("35e8f22d-40e3-454f-8c82-af50e81227e5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1237), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1238), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("37ca1513-b100-4af7-a7dc-5535c9041b60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2132), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2133), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("39609eae-9ca8-4a8d-93a5-52d28889e80b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1849), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1849), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a41011e-8561-47f7-9efd-f98a2b121c01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2191), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2192), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3a6933ca-e959-4cb9-a5c7-7c85776cd2c3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2261), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2261), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3c38425d-ca08-458e-8aa6-6dbde22d075f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2140), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2140), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3d1c757f-4c5c-41c4-9431-3d76f54405fb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1204), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1205), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f07700f-b59e-4e13-b8a6-e04872461ba3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1752), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1752), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3f51ce5d-7894-4332-9ce2-28f9bfffd9cc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1760), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1761), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("3fbc97b7-a830-4e59-a534-18e27c1659de"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1940), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1940), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("405afb0c-1252-4c8e-b1d7-193dc2ea61ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1443), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1444), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("40a02930-815a-4396-9893-7138128667d0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1782), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1783), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("424e2ade-0b15-4014-bd24-0e66e50c69a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1504), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1504), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("42c1ad57-365d-43c8-a746-503c9b723060"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1350), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1351), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("43d87e04-fddf-406d-9172-adffc4295be0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1875), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1875), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4737f612-058e-4491-829f-a336fb9136e1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1617), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1617), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("479b24dc-8673-4a71-ac3b-2b45d1669614"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1908), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1908), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("47c4a842-a333-4034-9741-a8aea34e6159"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1388), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1389), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("48e9e1a9-9233-495e-a5c5-c1398a433ad8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2169), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2170), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a71dd62-bef0-4d7b-a967-4ae93751a69b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1340), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1341), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4a79190f-0271-4e66-936f-8d4bfd10a5ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1223), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1223), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4affbb36-dbe5-483b-bdb9-3b21565e0658"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1529), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1530), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4baf1d59-6c43-4d72-a1f9-318e44cf1bae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1676), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1677), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c55b3a2-b325-4835-939e-5e3ef835bf66"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1606), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1606), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4c80e258-93f8-4631-b83b-d5a1254bbdca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1658), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1659), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4cdebe09-d9ab-4229-91e2-4ca9e6899089"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1775), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1775), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4d556e6a-4037-4c29-a40d-3fe284679693"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1470), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1471), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4e3e8c9e-58ed-4aab-acf8-b969e8d44cd5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1399), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1400), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb23b6a-a682-482d-b9c3-689f29d1856a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2010), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2010), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4eb79c3b-df7a-4c59-9d97-ac930ca623e6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1926), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1927), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("4ffb768c-34f2-4aea-ab97-497a3d5c9e32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1983), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1984), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50557d75-1849-4528-8b57-68a0eebb4877"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1726), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1727), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("50c22935-6a40-4b97-b7d5-e7acd32f7e19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1266), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1266), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("52f25a39-d50f-4e0e-9dd5-729c7f9c3fee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1548), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("53dbbec5-311a-49f3-9dc7-143b9f150eab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2310), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2311), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("54419442-e140-4642-ab30-af6e2dd74228"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1632), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1632), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("546f7053-f243-43f2-8479-c97165c1c97c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2359), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2360), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("556326f1-54e5-4c38-96ff-2a8782d03f59"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2121), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2121), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("559aef93-6209-456a-8b5c-d859e8c00e49"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2363), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2363), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56f2ea58-47f0-4229-8c22-b101d390bd7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1797), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1797), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("56faac0a-ccd0-4019-9581-108006412c95"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1714), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1715), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5712c2a5-7e44-4c16-8e98-1d647331f00d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1317), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1318), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("576c97af-fde1-408d-9042-b75448adc98d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2280), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2281), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("578e46f1-fb0b-4766-adb1-3204d0ee92c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1764), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1764), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5870d3d6-c36d-444a-bd48-0ee20b73c8dc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1957), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1957), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ae2fff6-23ec-46cc-a10e-7dd3d0c5cd1c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2144), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2145), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5b3ef045-3abc-48e2-8198-c31018c99481"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1250), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1251), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5ddcf642-c337-471b-bccf-d7feebbd417e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1414), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1414), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("5dfa3bcf-5f87-4ae7-82af-f4467905e4c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1269), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1270), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("61e24c11-d0d9-4e4c-9ba8-f44aab8bd573"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1428), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1429), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("62d21d9d-878c-4d7f-a814-feca5a481cee"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1800), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1801), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6307fe1e-287a-4e0d-b949-eb8696364f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1669), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1669), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6312a346-339e-43f1-bf4e-ab46a2669f80"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1392), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1392), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("635198f6-16c4-4033-a433-58cf962ffa6b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1362), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1362), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63c6725f-d1d1-4349-8320-275874d68594"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1824), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1824), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ce742c-4532-4211-9f8d-32ac44e1d557"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1767), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1767), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("63ef80ea-feb3-46e3-ae65-493ae81df639"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1337), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1337), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6416ef97-03b3-4a28-9fcf-195b92d5dc6e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1962), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1962), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("647e5d2d-589a-4caf-b777-2d3a8122cd2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1522), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1523), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("64e88c5b-31a0-4857-b96e-342b71572897"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2064), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2064), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("665021ae-e50e-4874-a502-7189380d5f53"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1944), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1944), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6694a4bb-7729-43d0-8b66-e904d79cc2b3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1887), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1888), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("674ced77-2991-46c0-8e65-abe2fe966c03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2166), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2166), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6828c5b8-50fe-41a2-8b69-6eaa22fd86f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2084), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2085), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("689f2c1c-6a26-4daa-aff2-2d223def8b52"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1518), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1519), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6a4af2a2-ed38-4e42-9e98-1d630f8c396d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2314), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2314), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6aacbf40-a3cc-4993-bb1a-3c92aa56366d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2239), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2240), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6bc62b7a-c062-48a1-846a-127626b977f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1895), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1895), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6d5c31f4-56f5-4cf3-a875-ad137c8293d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2097), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2098), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6e37a8c1-9a0d-4cce-a268-724bd1cd3d17"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1786), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1786), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f328079-ecd0-4b2e-8b37-e4f40f228f9a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1665), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1666), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6f7779fe-1ecc-4da1-8647-51226936afc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1845), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1846), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("6fac827e-b9bd-4ca2-90c4-6ec500a05e03"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1730), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1730), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70de2594-ecea-41ea-8fac-b0302c2b5e65"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1793), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1794), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("70eb906b-851b-4278-9540-2badcc279e10"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1970), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1970), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7168fb0c-1142-4924-b27d-d39831a75037"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1703), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1703), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("716fab26-5b55-4d46-a4de-10873d0625b4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1587), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1588), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("71c34eec-9b5f-49c7-a3be-ace36b45f15a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2210), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("72b1c7a9-b6d5-40a7-a699-bf6598047882"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1651), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1651), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73b108e5-328c-4160-95ba-c96bc45a73d9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1254), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1255), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("73fd24a5-dddd-4837-8c9e-027cec3175f9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1299), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1299), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74324406-77e4-4f78-b486-5ce18ecce5e3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1365), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1366), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("74836307-2d5d-46b6-a350-7fdf1a9bcee2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2265), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2265), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("754b4547-831d-48f5-a97a-efdd0b4cadc8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2040), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2040), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("766fde1a-e0f4-4a06-a565-749f0e62a512"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1477), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1478), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("771145fc-579e-4452-b331-11329dafab55"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1507), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1508), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("77517652-b41d-4954-bd95-6fa3554c03cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2247), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2248), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("778959a1-4baa-4f70-863f-92b4db0b1e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1613), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1614), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a3952d1-f745-4c48-ade4-a7a3483ac348"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1563), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1563), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7a52a66a-759d-4e7b-b0f7-d5a48c5fcba7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2295), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2296), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7d064556-e6b3-410a-801e-8682b2bb75c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1373), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1373), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("7fef5aaf-0232-4907-88ad-3d58e10c15d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1838), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1838), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("80d0ff9c-0d05-4727-a603-595dccd73f4b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1996), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1997), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81062b69-2d5e-4ff3-abc8-8b70fab4cc38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1556), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1556), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81423693-49b9-4554-a113-5db584f79a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1778), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1779), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8170d54f-b6c8-4bbc-9763-92b374544edd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2151), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2152), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("81b1e3f7-525e-4b24-8e4b-1342909ef6cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1313), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1314), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8434011f-6ae2-419c-a77e-220dfb21c028"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1904), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1904), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("860c0937-8adf-4860-a7c7-5791aa8f7f19"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1602), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1603), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8617a57c-d899-4320-b1e2-88c4b7704072"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1689), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1689), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("86668055-7b54-4cd3-aa7d-bfc0dc6b223e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2288), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2289), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("874eef90-39de-4367-a482-5f4d8c9d6f84"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2199), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2200), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("87af5a28-4975-4e91-8665-daabc185a403"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1707), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1708), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8bcd3442-31bd-40c6-a246-de59da0fe2ab"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2101), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2102), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c0b89fe-11fb-4196-a246-17a17f4592d8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1466), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1466), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c5eb361-3fb3-478e-afa2-97970139dd69"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1741), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1741), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("8c9be888-fbcb-4394-bc9e-3e27571254fc"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2321), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2322), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("924df98b-9eb0-4585-9376-87dbbda62e01"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1377), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1377), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("93111020-404c-4712-84cf-8febfda90bda"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2206), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2207), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9630a6a7-d51e-4912-82ec-3f9372dfbbd1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1537), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1537), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("969c634e-3f3a-419a-8526-e10edbd8c5ca"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1951), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1952), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("97357fcd-5559-4a09-a7ac-901e3ef851bf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1229), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1230), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9836be8a-a6d8-40fa-968e-c228f89bb914"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2292), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2292), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("986291f1-06f6-40b0-81db-ca076898991c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2229), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2229), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("99016a1e-3bed-4399-bc4e-203e6fcd8a5a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2051), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2052), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9976bd33-0939-4751-8906-a4dfa5383329"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1625), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9b109e35-f3f4-4cc4-b260-531ae5baa6b9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2243), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2243), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9df91660-cf3c-41bf-bf99-2d4c95f1eb50"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1591), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1591), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9e620384-0990-4704-b3ee-1414ad4b5d8f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1273), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1274), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("9eb802bb-91fd-4fbd-9d1b-e0a33f363e62"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1417), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1418), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a037fb68-185f-404c-a68d-b5191fcde40f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1948), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1948), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a0c8afb7-71e9-49ac-97e4-e30434d41bd4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1262), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1262), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a377d9c8-b36c-4398-b94d-dffd2b9b7e2f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1662), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1662), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a4ecc6d6-aadd-4fdd-b2d8-211bffe8a60a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2114), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2114), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a6cb4b9e-865b-4a10-8b27-fc6e18053946"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1437), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7ced9df-e3f3-421c-b52e-e851561612cd"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1421), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1422), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a7fc976d-e924-4b1a-80ba-25fa11c6eada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2117), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2118), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a8181d58-e6fb-4d84-9ba8-1d3426d63b25"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2048), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2048), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("a9d7e3a9-1d94-48bf-a6b6-f2b4ea642c6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1425), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1425), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("abbd7ea3-5e9f-429b-9bc9-2eac05e365b0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1321), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1321), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ac4fd120-5184-43d2-bcbc-3aa661029deb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2068), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2068), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("acf5ce30-c463-45e1-9b36-0ae006ebdedf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1583), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1584), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ada316d2-d3bf-4f9e-a768-bc2485a08ada"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1644), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1644), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b18fb01b-496b-4c19-b834-ccfb9247fb16"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1455), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1455), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b1f86138-bd5e-4078-911b-4d81c78e4414"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2129), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2129), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b296bea3-8590-4c82-8060-87feed620d5f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2203), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2203), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b482e424-abfd-46ab-8919-015fe2cede4e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2303), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2304), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b5533594-0720-40fd-b3b2-52e3497b8b40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2336), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2336), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b586ac34-995f-4743-82f0-21f1f4e3d76d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1515), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1515), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b7c55d98-2ca5-498d-83c5-65821f69b0a9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2005), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2005), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b81b6fa4-1d67-4843-8001-d1236c0c046e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1410), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1411), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("b99ec1be-734e-4f3f-b1d2-52dffc9ce612"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1852), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1853), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ba5c3b79-507e-4afd-bb65-641c9d859763"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1474), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1474), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bc79b37f-d82d-4f32-8e1f-9834734f114e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2161), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2162), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bce7924e-ffbb-4434-af88-e3b147b502a7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2180), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2181), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bd95782e-0020-4fa8-ac92-ff34a5438c2a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1440), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1440), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("becfbad6-9da2-48e7-8841-82609bb8cac9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1610), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bf4b78f7-863c-42d7-9465-fa11a6a9b175"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2299), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2299), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("bfc76c18-4434-4aae-924a-d8dd126e4014"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1748), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1748), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c0cb789d-f745-4a0d-b166-763c499563c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2257), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2258), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c43440ae-99c5-4722-84f9-29dc752e81d5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2060), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2061), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c5763f60-c5de-42d1-b97e-d78745e281f1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1883), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1883), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c6c1fe44-2147-41ca-8100-48c1179545a0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1333), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1333), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8aad664-4329-40df-bdae-404d16ad28e0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2325), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2325), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c8b369a5-b9c1-48f7-9aeb-e56e39eea7ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2184), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2185), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("c95f54b9-743a-43c5-9336-753646b2bae0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1551), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1552), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cceb2b1b-d848-41a2-b987-1c256c1caf3d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1966), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1966), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cede99f7-0fac-416c-aca0-359b1ceb6665"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1723), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1723), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf34f086-c4f2-49c2-9b12-8cec89f76d73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2225), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2226), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cf86d807-0488-4fdc-86cd-fbc1849c2232"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2232), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2233), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("cfc21067-9cb1-4e8f-bd06-698befaad209"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1215), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1216), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d019d451-4ef0-4a26-ac7c-3fe0040a03f8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1541), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1541), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d03dff77-9c74-49bb-a5a7-10f00a52eadf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1719), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1719), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d04b3225-4455-4dbf-879c-52521881ba32"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1680), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1680), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d0eb085d-9415-4f52-aee2-3d2631ee17e2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1595), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d10f35d9-7528-41ed-8362-25a13e3d19c7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2056), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2057), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d1569f7a-17b0-4499-86bc-8da3f62d78df"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2089), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2090), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d21fb3f4-063a-464c-8732-c5a8aeca001d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1744), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1745), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d28d60b7-a11f-420d-9b78-8978221a12b7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2270), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2270), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d3c64bb3-2e70-4eac-b1fb-5bf4fdc374c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1211), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1211), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d472d4c8-b215-4477-8f0d-687e8f698158"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2136), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2136), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d48e3d13-de43-429a-bbc3-0cdcc1ca8ea1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1462), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1462), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d502b3f2-936d-449b-90cd-a783fb2942f4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1451), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1451), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d57947e1-16c0-4b9d-9573-3d2767815451"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1532), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1533), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d59b6e17-2905-46f7-977b-545cdbf9251e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1355), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1355), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d5b77b5b-ace9-4c54-af3e-bae4449696c8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1284), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1285), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7919a88-1042-4951-991d-be49aba34367"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1572), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1573), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d7ab6445-47e1-4602-8c29-66d500b5ca51"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2075), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2076), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d927c02d-5ebc-4b7b-9b7f-f7fab5ca3bc2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2110), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2110), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d93ff951-cc6d-4c57-addf-cf34965074c9"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1579), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1580), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("d98bab5e-1b3d-4138-b97c-b71db86d30c0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1492), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1493), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("dbe48e80-b8d1-4140-b030-8a1c348ac3c6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1481), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1481), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("de36b74c-6770-4a9a-8e53-3a032d3170af"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2236), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2236), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("deed3198-0bfa-4313-9919-64090878ff40"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1860), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1860), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0b42e50-0db1-440a-b392-7056af38f492"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1684), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1684), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e0f45b74-6c7a-4c6f-b913-41d887443cd3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1328), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1328), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e1a611db-e710-467c-9404-de8e82dfcdc7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1288), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1288), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e4866ef8-7a9f-49ea-af3d-2f2adc388255"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1526), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1526), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e49dcd0d-3820-4c45-9990-3b799d0b733b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1834), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1834), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e5be72d6-38eb-44c4-b087-6e1bc0120733"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1544), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1545), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e6056baa-29fe-43b4-bc21-e0c8a89ec112"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2366), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2367), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7274bb8-ab3c-4138-b091-9ddddef4499e"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1699), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1700), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7300f3a-c370-43d5-b268-44286a302c60"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1980), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1980), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e73c2a6a-ca1e-4149-b692-e8fb3cc67d48"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1458), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1459), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e7fd9c3b-207a-426d-829c-5864087b0ceb"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1987), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1988), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e88eef15-2e0c-42c2-a5f5-4cba09df8b7d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2332), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2332), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("e964019e-39bf-4477-ba06-9cff9d3e44ae"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2277), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2277), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eae2509e-0b08-42c6-9123-205c725291a2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2318), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2318), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb447e65-d19d-491e-b4bf-a0517a64b79b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1233), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1234), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb5281e2-efc5-444c-a92d-1a2603721b4f"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1819), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1820), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eb7554df-f80e-4194-886d-bee5babdd62c"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1246), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1247), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("eba41453-c8c2-491d-bb86-9598de7ffab2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1385), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1385), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ec289217-c6b7-4837-8b8a-2cbc9fbc5df0"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1447), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1447), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ed375f05-4451-42fa-bebe-96c194fcfddf"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2093), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2094), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ee736134-e8a9-4f6b-9057-aefe27a222d2"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1628), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1629), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ef4b1a02-bf0c-4aee-bf72-0fb21a303bc1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1816), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1816), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f30ae7c6-9f2d-42d9-9ba4-9a44a658fb6d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1790), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1790), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f318741a-31bf-41ad-9e30-627955fbcefe"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2001), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2001), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48d909e-a7ac-4990-b8da-2bc87429e7a5"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1829), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1830), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f48df6b4-ada9-4504-9a4a-ef68a0edaf8d"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1891), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1891), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f4f8d8c5-735c-42f3-b534-bf92030606d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1576), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1576), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f5c0457d-d3a6-4b87-931a-0ccdc4bdb343"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2195), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2196), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f715b6bb-91a0-452a-a589-0c4500aa3a73"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1306), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1307), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f7c81ff9-6224-42ea-8080-eed6479b334a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1711), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1711), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8924f7d-95dc-4e9f-b092-d0ad5046c1a1"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2176), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(2177), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("f8d3db4c-f63f-4c2a-84bf-174411fe1e22"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1755), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1755), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("fb03709a-af48-452d-b711-d5990db0fa38"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1496), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1496), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "ObjectColumns",
                keyColumn: "Id",
                keyValue: new Guid("ff23c77a-22fa-4e27-86f6-a71cb4e43e36"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1871), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1872), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1148), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1149), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1104), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1105), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1146), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1146), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1137), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1138), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1141), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1141), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1143), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1144), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1135), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1135), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1132), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1133), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1154), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1155), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1120), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1120), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1113), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1114), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1107), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1108), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1122), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1123), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1117), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1117), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1157), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1157), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1125), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1125), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1151), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1151), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1159), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1160), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1110), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1111), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "SpecificationObjects",
                keyColumn: "Id",
                keyValue: new Guid("e44e12a4-df37-401e-afc9-08024be3991a"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1100), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1101), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "DecryptionManualTriggerUrl", "LandingManualTriggerUrl", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1021), new TimeSpan(0, 0, 0, 0, 0)), "Update this => environment specific", "Update this => environment specific", new DateTimeOffset(new DateTime(2023, 10, 3, 15, 30, 9, 269, DateTimeKind.Unspecified).AddTicks(1029), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
