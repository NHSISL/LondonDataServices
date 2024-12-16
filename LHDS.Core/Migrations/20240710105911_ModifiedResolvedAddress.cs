// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedResolvedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMatched",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "JsonPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedBuildingNumber",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDepartmentName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDependentThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedDoubleDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedJsonPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedOrganisationName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedPostCode",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedPostTown",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedSubBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchedThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "PostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "MatchedUPSN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "UPSN");

            migrationBuilder.RenameColumn(
                name: "MatchedUPRN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "UPRN");

            migrationBuilder.RenameColumn(
                name: "MatchAlgorithmEnum",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "RetryCount");

            migrationBuilder.RenameColumn(
                name: "IsProcessed",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "IsProcessing");

            migrationBuilder.AlterColumn<string>(
                name: "PostCode",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "AddressFormatQuality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Algorithm",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingNumber",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classification",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DependentThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoubleDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchPattern",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Matched",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCodeQuality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostTown",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualifier",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressFormatQuality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Algorithm",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "BuildingName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Classification",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "DependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "DependentThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "DoubleDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "MatchPattern",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Matched",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "PostCodeQuality",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "PostTown",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Qualifier",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "SubBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.DropColumn(
                name: "Thoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "UPSN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "MatchedUPSN");

            migrationBuilder.RenameColumn(
                name: "UPRN",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "MatchedUPRN");

            migrationBuilder.RenameColumn(
                name: "RetryCount",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "MatchAlgorithmEnum");

            migrationBuilder.RenameColumn(
                name: "IsProcessing",
                schema: "Addresses",
                table: "ResolvedAddress",
                newName: "IsProcessed");

            migrationBuilder.AlterColumn<string>(
                name: "PostCode",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMatched",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JsonPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedBuildingNumber",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDepartmentName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDependentThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedDoubleDependentLocality",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedJsonPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedOrganisationName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedPostCode",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedPostTown",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedPostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedSubBuildingName",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchedThoroughfare",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalAddress",
                schema: "Addresses",
                table: "ResolvedAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
