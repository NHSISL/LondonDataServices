using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedObjectColumnDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 8, 30, 21, 37, 28, 557, DateTimeKind.Unspecified).AddTicks(898), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 8, 30, 21, 37, 28, 557, DateTimeKind.Unspecified).AddTicks(909), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "Id",
                keyValue: new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"),
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 8, 30, 12, 14, 29, 352, DateTimeKind.Unspecified).AddTicks(1362), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 8, 30, 12, 14, 29, 352, DateTimeKind.Unspecified).AddTicks(1369), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}
