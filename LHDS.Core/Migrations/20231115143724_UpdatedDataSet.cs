// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDataSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "ActiveFrom", "ActiveTo", "DataSetAliases", "DataSourceType", "IsActive" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PrimaryCareEMISDEV", "PrimaryCareEMISDEV", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Configuration",
                table: "DataSets",
                keyColumn: "Id",
                keyValue: new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"),
                columns: new[] { "ActiveFrom", "ActiveTo", "DataSetAliases", "DataSourceType", "IsActive" },
                values: new object[] { null, null, "", "", false });
        }
    }
}
