// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedResolvedAddressWithExtendedMatchDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UPSN",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedUPSN");

            migrationBuilder.RenameColumn(
                name: "UPRN",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "MatchedUPRN");

            migrationBuilder.AddColumn<string>(
                name: "MatchedBuildingName",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedBuildingNumber",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDepartmentName",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDependentLocality",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDependentThoroughfare",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDoubleDependentLocality",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedOrganisationName",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedPostCode",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedPostTown",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedSubBuildingName",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedThoroughfare",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchedBuildingName",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedBuildingNumber",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDepartmentName",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDependentLocality",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDependentThoroughfare",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDoubleDependentLocality",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedOrganisationName",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedPostCode",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedPostTown",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedSubBuildingName",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedThoroughfare",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "MatchedUPSN",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "UPSN");

            migrationBuilder.RenameColumn(
                name: "MatchedUPRN",
                schema: "UPRN",
                table: "ResolvedAddress",
                newName: "UPRN");
        }
    }
}
