using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSubscriberAgreementToHaveSupplierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                schema: "Configurations",
                table: "SubscriberAgreements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                schema: "Configurations",
                table: "SubscriberAgreements");
        }
    }
}
