using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class InititlalMmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IngestionTrackings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EncryptedFileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DecryptedFileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Decrypted = table.Column<bool>(type: "bit", nullable: false),
                    LastSeen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FileDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RecordCount = table.Column<int>(type: "int", nullable: false),
                    EncryptedFileSize = table.Column<int>(type: "int", nullable: false),
                    DecryptedFileSize = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionTrackings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandingManualTriggerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngestionTrackingId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audits_IngestionTrackings_IngestionTrackingId",
                        column: x => x.IngestionTrackingId,
                        principalTable: "IngestionTrackings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_IngestionTrackingId",
                table: "Audits",
                column: "IngestionTrackingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "IngestionTrackings");
        }
    }
}
