using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAddressWithIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Address_IsErrored",
                schema: "UPRN",
                table: "Address",
                column: "IsErrored");

            migrationBuilder.CreateIndex(
                name: "IX_Address_IsNormalised",
                schema: "UPRN",
                table: "Address",
                column: "IsNormalised");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PostCode",
                schema: "UPRN",
                table: "Address",
                column: "PostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Address_Processing",
                schema: "UPRN",
                table: "Address",
                column: "Processing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Address_IsErrored",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_IsNormalised",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_PostCode",
                schema: "UPRN",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_Processing",
                schema: "UPRN",
                table: "Address");
        }
    }
}
