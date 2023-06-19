using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class supplierseeddataupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate" },
                values: new object[] { "System", new DateTimeOffset(new DateTime(2023, 6, 19, 11, 10, 2, 273, DateTimeKind.Unspecified).AddTicks(7421), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateTimeOffset(new DateTime(2023, 6, 19, 11, 10, 2, 273, DateTimeKind.Unspecified).AddTicks(7434), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate" },
                values: new object[] { null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
