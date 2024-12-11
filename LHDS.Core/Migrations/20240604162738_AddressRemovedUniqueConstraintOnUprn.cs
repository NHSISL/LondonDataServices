// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddressRemovedUniqueConstraintOnUprn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address",
                column: "UPRN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UPRN",
                schema: "UPRN",
                table: "Address",
                column: "UPRN",
                unique: true,
                filter: "[UPRN] IS NOT NULL");
        }
    }
}
