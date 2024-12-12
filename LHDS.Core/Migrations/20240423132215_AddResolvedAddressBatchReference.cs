// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddResolvedAddressBatchReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsErrorred",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                newName: "IsError");

            migrationBuilder.AddColumn<Guid>(
                name: "BatchReference",
                schema: "UPRN",
                table: "ResolvedAddress",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchReference",
                schema: "UPRN",
                table: "ResolvedAddress");

            migrationBuilder.RenameColumn(
                name: "IsError",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                newName: "IsErrorred");
        }
    }
}
