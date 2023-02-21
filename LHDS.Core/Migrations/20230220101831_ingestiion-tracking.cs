// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ingestiiontracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DecryptedFileSize",
                table: "IngestionTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EncryptedFileSize",
                table: "IngestionTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FileCount",
                table: "IngestionTrackings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FileDeleted",
                table: "IngestionTrackings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastSeen",
                table: "IngestionTrackings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecryptedFileSize",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "EncryptedFileSize",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "FileCount",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "FileDeleted",
                table: "IngestionTrackings");

            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "IngestionTrackings");
        }
    }
}
