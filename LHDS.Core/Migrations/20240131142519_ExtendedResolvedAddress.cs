// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedResolvedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF OBJECT_ID('LDS.Batch', 'U') IS NOT NULL
                BEGIN
                    -- Drop foreign key constraints related to temporal history table
                    ALTER TABLE LDS.Batch SET (SYSTEM_VERSIONING = OFF);
                    DROP TABLE LDS.BatchHistory;

                    -- Drop the temporal table
                    DROP TABLE LDS.Batch;
                END");

            migrationBuilder.CreateTable(
                name: "ResolvedAddress",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UPRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UPSN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchAlgorithmEnum = table.Column<int>(type: "int", nullable: false),
                    IsMatched = table.Column<bool>(type: "bit", nullable: false),
                    MatchedWithPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchedWithJsonPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolvedAddress", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResolvedAddress",
                schema: "UPRN");
        }
    }
}
