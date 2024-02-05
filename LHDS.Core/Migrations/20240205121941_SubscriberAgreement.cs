using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class SubscriberAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Configurations");

            migrationBuilder.CreateTable(
                name: "SubscriberAgreements",
                schema: "Configurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierSharingAgreementShortName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SupplierSharingAgreementGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FtpUserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FtpPublicKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    GpgPublicKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastPollStartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastPollEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberAgreements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberAgreements_SupplierSharingAgreementShortName",
                schema: "Configurations",
                table: "SubscriberAgreements",
                column: "SupplierSharingAgreementShortName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriberAgreements",
                schema: "Configurations");
        }
    }
}
