using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptOuts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhsNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BatchReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UniqueReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CacheTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastSentToMesh = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptOuts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdsAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsAudits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FriendlyName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandingManualTriggerUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DecryptionManualTriggerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "IngestionTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_IngestionTrackings_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngestionTrackingId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 450, nullable: false),
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

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DecryptionManualTriggerUrl", "Description", "FriendlyName", "LandingManualTriggerUrl", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 7, 4, 9, 43, 10, 955, DateTimeKind.Unspecified).AddTicks(8577), new TimeSpan(0, 0, 0, 0, 0)), "Update this => environment specific", "Emis Supplier", "EMIS", "Update this => environment specific", "EMIS", "System", new DateTimeOffset(new DateTime(2023, 7, 4, 9, 43, 10, 955, DateTimeKind.Unspecified).AddTicks(8588), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Audits_IngestionTrackingId",
                table: "Audits",
                column: "IngestionTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackings_FileName",
                table: "IngestionTrackings",
                column: "FileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackings_SupplierId",
                table: "IngestionTrackings",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "OptOuts");

            migrationBuilder.DropTable(
                name: "PdsAudits");

            migrationBuilder.DropTable(
                name: "IngestionTrackings");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
