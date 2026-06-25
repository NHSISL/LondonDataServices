using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToUprnFileLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressToUprnFileLogs",
                schema: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EntryCount = table.Column<int>(type: "int", nullable: false),
                    EntriesMatched = table.Column<int>(type: "int", nullable: false),
                    EntriesUnmatched = table.Column<int>(type: "int", nullable: false),
                    ErrorRowCount = table.Column<int>(type: "int", nullable: false),
                    DateReceived = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateProcessed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    SuccessStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedWhen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedWhen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressToUprnFileLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressToUprnFileLogs_FileName",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_AddressToUprnFileLogs_SuccessStatus",
                schema: "Addresses",
                table: "AddressToUprnFileLogs",
                column: "SuccessStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressToUprnFileLogs",
                schema: "Addresses");
        }
    }
}
