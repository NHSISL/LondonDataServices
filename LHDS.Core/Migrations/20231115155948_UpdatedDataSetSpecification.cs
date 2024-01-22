// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDataSetSpecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CreatedBy", "CreatedDate", "DataSetId", "DateImplemented", "DateReleased", "DateSuperseded", "EntityChangeSynchronisation", "IsActive", "IsMultiAuthorPerBatch", "IsPublished", "Notes", "OurSpecificationVersion", "PresededById", "SupersededById", "SupplierSpecificationVersion", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "", true, true, true, "This is a test dataset specification", "1.0", null, null, "7.0", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"));
        }
    }
}
