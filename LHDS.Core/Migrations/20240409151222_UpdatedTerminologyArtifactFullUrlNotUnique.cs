// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTerminologyArtifactFullUrlNotUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TerminologyArtifacts_FullUrl",
                schema: "Ontology",
                table: "TerminologyArtifacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TerminologyArtifacts_FullUrl",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                column: "FullUrl",
                unique: true);
        }
    }
}
