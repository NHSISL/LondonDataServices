// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class RenameAddressToUprnFileLogAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedWhen",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedWhen",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                newName: "UpdatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                newName: "CreatedWhen");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                newName: "UpdatedWhen");
        }
    }
}
