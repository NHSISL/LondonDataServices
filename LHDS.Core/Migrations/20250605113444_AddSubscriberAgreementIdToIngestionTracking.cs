using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriberAgreementIdToIngestionTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackings_SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings",
                column: "SubscriberAgreementId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngestionTrackings_SubscriberAgreements_SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings",
                column: "SubscriberAgreementId",
                principalSchema: "Configurations",
                principalTable: "SubscriberAgreements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngestionTrackings_SubscriberAgreements_SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropIndex(
                name: "IX_IngestionTrackings_SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "SubscriberAgreementId",
                schema: "Ingestion",
                table: "IngestionTrackings");
        }
    }
}
