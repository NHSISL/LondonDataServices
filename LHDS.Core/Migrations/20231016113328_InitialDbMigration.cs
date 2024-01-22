using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LDS");

            migrationBuilder.EnsureSchema(
                name: "Configuration");

            migrationBuilder.EnsureSchema(
                name: "Ingestion");

            migrationBuilder.EnsureSchema(
                name: "Patient");

            migrationBuilder.CreateTable(
                name: "Batch",
                schema: "LDS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    BusinessKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SourceSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    StartDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EndDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "DataTypes",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTypes", x => x.Id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "OptOuts",
                schema: "Patient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhsNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BatchReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UniqueReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CacheTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastSentToMesh = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptOuts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pds",
                schema: "Patient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FriendlyName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataSets",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSetName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSetAliases = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSetAuthor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SpecifiedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsNationallySpecified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CollectedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsNationallyCollected = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ActiveTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSets_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Configuration",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "IngestionTrackings",
                schema: "Ingestion",
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
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngestionTrackings_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Configuration",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataSetSpecifications",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupplierSpecificationVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    OurSpecificationVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsMultiAuthorPerBatch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EntityChangeSynchronisation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DateReleased = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DateImplemented = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DateSuperseded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupersededById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PresededById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ActiveTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSetSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSetSpecifications_DataSetSpecifications_PresededById",
                        column: x => x.PresededById,
                        principalSchema: "Configuration",
                        principalTable: "DataSetSpecifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DataSetSpecifications_DataSetSpecifications_SupersededById",
                        column: x => x.SupersededById,
                        principalSchema: "Configuration",
                        principalTable: "DataSetSpecifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DataSetSpecifications_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalSchema: "Configuration",
                        principalTable: "DataSets",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "IngestionTrackingAudits",
                schema: "Ingestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngestionTrackingId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 450, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionTrackingAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngestionTrackingAudits_IngestionTrackings_IngestionTrackingId",
                        column: x => x.IngestionTrackingId,
                        principalSchema: "Ingestion",
                        principalTable: "IngestionTrackings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecificationObjects",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataSetSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupplierObjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    OurObjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ObjectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    InterchangeProtocol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsPushedToUs = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsPulledByUs = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DeletionHandling = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsSubmissionHeaderObject = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsTransactionLog = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationObjects_DataSetSpecifications_DataSetSpecificationId",
                        column: x => x.DataSetSpecificationId,
                        principalSchema: "Configuration",
                        principalTable: "DataSetSpecifications",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "ObjectColumns",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SpecificationObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupplierColumnName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    OurColumnName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    ColumnDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    OrdinalPosition = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PopulatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    FhirDataType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SqlDataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Length = table.Column<int>(type: "int", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Precision = table.Column<int>(type: "int", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    Scale = table.Column<int>(type: "int", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupplierDateFormat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsWatermark = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsSequencing = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsBusinessKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsUniqueRecordKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsVersionHashElement = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsSenderCode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsAuthorCode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsRelatedOrganisationId = table.Column<bool>(type: "bit", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsDeleteFlag = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsSensitiveRecordMarker = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    IsPersonConfidentialData = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PersonConfidentialDataType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    MaskingMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CodeSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PartitionColumnLevel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    DataTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectColumns_SpecificationObjects_SpecificationObjectId",
                        column: x => x.SpecificationObjectId,
                        principalSchema: "Configuration",
                        principalTable: "SpecificationObjects",
                        principalColumn: "Id");
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "Suppliers",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "FriendlyName", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7202), new TimeSpan(0, 0, 0, 0, 0)), "Emis Supplier", "EMIS", "EMIS", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7210), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSets",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CollectedBy", "CreatedBy", "CreatedDate", "DataSetAliases", "DataSetAuthor", "DataSetName", "DataSourceType", "IsActive", "SpecifiedBy", "SupplierId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), null, null, "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7267), new TimeSpan(0, 0, 0, 0, 0)), "EMIS", "EMIS", "PrimaryCareEMIS", "", false, "EMIS", new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7268), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CreatedBy", "CreatedDate", "DataSetId", "DateImplemented", "DateReleased", "DateSuperseded", "EntityChangeSynchronisation", "Notes", "OurSpecificationVersion", "PresededById", "SupersededById", "SupplierSpecificationVersion", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7283), new TimeSpan(0, 0, 0, 0, 0)), new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), null, null, null, "", "", "1", null, null, "7", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7284), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "SpecificationObjects",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DataSetSpecificationId", "DeletionHandling", "InterchangeProtocol", "ObjectDescription", "OurObjectName", "SupplierObjectName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7340), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Appointment_Slot", "Appointment_Slot", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7341), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7302), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Agreements_SharingOrganisation", "Agreements_SharingOrganisation", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7303), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7338), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Admin_Patient", "Admin_Patient", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7338), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7330), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Admin_OrganisationLocation", "Admin_OrganisationLocation", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7331), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7333), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Admin_Organisation", "Admin_Organisation", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7333), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7335), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "CareRecord_ObservationReferral", "CareRecord_ObservationReferral", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7336), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7328), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "CareRecord_Observation", "CareRecord_Observation", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7328), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7325), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Coding_ClinicalCode", "Coding_ClinicalCode", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7325), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7346), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Admin_UserInRole", "Admin_UserInRole", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7346), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7316), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Appointment_Session", "Appointment_Session", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7317), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7310), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Prescribing_DrugRecord", "Prescribing_DrugRecord", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7311), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7305), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Audit_PatientAudit", "Audit_PatientAudit", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7305), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7319), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Coding_DrugCode", "Coding_DrugCode", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7319), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7314), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Prescribing_IssueRecord", "Prescribing_IssueRecord", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7314), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7348), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "CareRecord_Problem", "CareRecord_Problem", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7349), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7322), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "CareRecord_Diary", "CareRecord_Diary", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7323), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7343), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Audit_RegistrationAudit", "Audit_RegistrationAudit", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7343), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7350), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Appointment_SessionUser", "Appointment_SessionUser", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7351), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7308), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "CareRecord_Consultation", "CareRecord_Consultation", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7308), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7299), new TimeSpan(0, 0, 0, 0, 0)), new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), "", "", "", "Admin_Location", "Admin_Location", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7299), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("016b7c2a-3cb9-4433-a79e-b55ca988eb57"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7435), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ActualDurationInMinutes", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "int", "ActualDurationInMinutes", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7436), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("016f00d9-f56e-4610-a38d-a3c49023d4c1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8040), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8041), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("01abe9ac-bbd0-41e7-a8c3-b7c1a967cd1a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7818), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredDate", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "date", "EnteredDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7818), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("024eb41e-404f-461f-ad10-fcb9a6c37fe4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8378), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModifiedTime", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "time", "ModifiedTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8379), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("02fdb2c0-8fb5-4a4a-b1e0-68614da128e3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8088), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ItemType", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "varchar", "ItemType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8088), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("04c59f48-ba31-4809-94bb-f4f5e1cf4460"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7684), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OpenDate", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "date", "OpenDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7685), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("053eade2-698d-4cbd-b84b-213adb1ea3f5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8071), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModifiedTime", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "time", "ModifiedTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8072), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("05803762-b255-48b9-bdae-802e7a6aaf05"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8316), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OriginalTerm", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "varchar", "OriginalTerm", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8317), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("07c3cca3-7eb1-451e-8087-ee1f2aa733ab"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7481), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsActivated", "", "", "", null, null, new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "bit", "IsActivated", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7481), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("08ae5632-37ad-4f2a-bfd2-a3ef3c73d6cb"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7670), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationName", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "varchar", "OrganisationName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7670), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("09b1725d-18d8-4222-8c43-a4bea3ba8b06"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7673), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CCGOrganisationGuid", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "uniqueidentifier", "CCGOrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7674), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("09b6077f-74a6-4cc0-9992-0c7723d81416"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7850), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NationalCodeCategory", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "NationalCodeCategory", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7851), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("09f8009d-8453-4ce5-b7b4-8ed6eb4b2d31"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7868), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7869), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("09fb5c58-0336-4b20-b521-83e5cd8b6b49"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8506), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "FaxNumber", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "FaxNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8507), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0a5fc198-19b8-43ae-b798-45506c385e20"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8084), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModeType", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "varchar", "ModeType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8085), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0a946e6b-f495-4cd4-8639-bc17173d1864"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7746), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralUBRN", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "char", "ReferralUBRN", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7747), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0ad1dd7b-4d2a-417e-a6b2-6135af7c19b4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8239), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Comment", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "Comment", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8239), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0bbbd975-da17-4340-95ae-34646282eba2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8294), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDatePrecision", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "varchar", "EffectiveDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8294), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0bc8f1e1-3583-4ae2-8eac-6df87422eb79"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7554), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ExternalUsualGPOrganisation", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "ExternalUsualGPOrganisation", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7554), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0ca7ae3a-036d-49a4-bb59-4cf3b5435ce3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8019), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredByUserInRoleGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "EnteredByUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8019), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0cfc4aff-ce97-4c48-8258-0ba55e63c49c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7990), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CancellationDate", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "date", "CancellationDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7990), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("0e166c8f-eb6c-43d0-b12c-f49d44953e6a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8103), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8105), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("0ef487f9-476e-4ec8-899d-541478430e16"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8033), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Quantity", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "decimal", "Quantity", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8033), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0f37bbe4-afbf-474b-815e-f0dbdb653540"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8275), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ParentProblemRelationship", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "ParentProblemRelationship", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8276), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0f5f262d-55d1-4611-9b1f-355ec4aab0d9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8146), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDate", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "date", "EffectiveDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8147), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0fe408cb-5610-461b-8260-510814bc54bb"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7780), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Value", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "decimal", "Value", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7781), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1091b3d3-a4a8-44b8-b4e5-c10c1014dc3f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8200), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Quantity", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "decimal", "Quantity", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8201), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("1481245d-c122-4890-b5d4-ae87ff63c62a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8510), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "LocationGuid", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "uniqueidentifier", "LocationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8510), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("15096c24-3ea9-4f19-a7a0-ee4c5a09dd12"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8349), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8349), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("15422b24-e865-4db5-8536-3cedb69e3efc"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7510), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ResidentialInstituteCode", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "char", "ResidentialInstituteCode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7510), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("15d5d359-d192-43ff-b7bd-b4ad0406146e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8051), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumberOfIssuesAuthorised", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "tinyint", "NumberOfIssuesAuthorised", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8051), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("16143ebe-976a-4a9e-805d-8f85265e1003"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8429), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ConsultationSourceTerm", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "varchar", "ConsultationSourceTerm", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8430), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("17b7fdc6-616c-4fa7-8ee6-f970443d0c11"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8212), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "QuantityUnit", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "varchar", "QuantityUnit", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8213), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("18d0f511-1978-46cc-9721-0475433bccc1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7986), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredTime", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "time", "EnteredTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7987), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1b3388cf-2e88-431e-815f-b8eef77568f1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7579), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SpineSensitive", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "bit", "SpineSensitive", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7580), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1c34861b-aec0-4508-99a9-c4f79a76abfc"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7831), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumericRangeHigh", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "float", "NumericRangeHigh", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7832), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1c71fe1b-41f4-4e7c-aa5f-c1ecbcc7455a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8204), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8206), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("1d11df66-c4a7-4e85-84bb-77a5fe923590"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8382), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8382), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("1d661e5a-fcf8-4dec-8634-07f5f6c3e741"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7488), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7489), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1e11b982-faad-4e5c-a8bd-95f1d4b5c648"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7740), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralSourceOrganisationGuid", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "uniqueidentifier", "ReferralSourceOrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7740), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("1f381e8f-19e4-4685-a84b-6c7f41ca0480"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8248), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LastReviewDatePrecision", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "LastReviewDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8249), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("223b3cc1-56f6-44ed-89cf-b13b06e49e99"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8194), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DrugRecordGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "DrugRecordGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8195), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("233b0b2f-4620-4adb-8107-8a7e3fea5b1c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7565), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Title", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "Title", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7565), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("23d3274a-f5e4-47a2-8ccb-a4d5d5aa84b2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7802), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredTime", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "time", "EnteredTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7803), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("25cb156f-86d0-4c68-9583-6ddbe08042cf"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7414), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7415), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("261902dc-c53e-4017-b0ab-7e350ce62c7d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8185), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDatePrecision", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "varchar", "EffectiveDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8186), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("26331b24-6f9f-4f6f-bfcb-c8ae81dca9a8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8375), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8375), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("2655335d-4a67-4be7-a8f1-860a7c7aec20"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8092), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8092), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("28246209-2a86-456c-97a5-00eb5ed89276"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7983), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDate", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "date", "EffectiveDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7983), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("28ce4c02-8c50-465c-b2e9-a6065eb5d522"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8189), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredDate", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "date", "EnteredDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8190), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2afb470a-7b78-4a3f-afab-fc817dcae6e8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8528), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Town", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "Town", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8528), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2b4b318a-f79f-4eb6-9c8d-90bbc1a5baa2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7913), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "JobCategoryName", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "varchar", "JobCategoryName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7914), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2b762785-2318-4407-b52b-c93142fc067f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7587), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientTypeDescription", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "PatientTypeDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7588), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2c1c993a-72f3-4bf3-ad8e-cbfcc0cdd323"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8334), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ConsultationGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "ConsultationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8335), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2cb2c37e-677c-4918-93b9-bdb4a18fdc99"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7825), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ConsultationGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "ConsultationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7825), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("2dc110e6-417f-41d1-97aa-e1d8bcae2126"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7451), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "SlotGuid", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "uniqueidentifier", "SlotGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7451), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("2de218bf-f65c-44c2-8b97-e813931aa669"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7477), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LastModifiedDate", "", "", "", null, null, new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "datetime", "LastModifiedDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7478), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2e7f0550-7ddd-4035-899f-b5b0c01a2f43"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8514), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumberAndStreet", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "NumberAndStreet", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8514), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2f2df5d0-141e-4cf8-83a3-13c520078e51"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8468), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CloseDate", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "date", "CloseDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8468), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2fe17d9f-3ff3-41a9-a75f-f9776e8278f1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7613), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Sex", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "char", "Sex", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7613), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("310cdb68-574f-4904-ac15-589db18d93a1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8162), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8163), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("31b8a2cb-7ea8-4212-ba08-93f07f52a926"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7872), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SnomedCTDescriptionId", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "bigint", "SnomedCTDescriptionId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7872), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("3277f9a2-be19-44f7-a320-4e57b08186c7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7532), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7532), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("32f7d725-972f-4c61-9cd7-120c4575f6da"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7495), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Town", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "Town", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7496), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3457368f-3486-4598-b75e-6c4862acc273"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8502), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OpenDate", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "date", "OpenDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8503), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3493ddcc-e3db-4ab5-bc2c-bd0c53910fd9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8441), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Complete", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "bit", "Complete", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8441), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3504618b-fbdf-4d1a-8ce3-4beaeb9f36b1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7946), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Description", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "varchar", "Description", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7947), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("352d7e99-e0a3-417a-a9f4-41764c8037d3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8491), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Postcode", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "Postcode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8492), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("354a00f1-919f-46e5-bc27-558864dd6282"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7543), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DateOfRegistration", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "date", "DateOfRegistration", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7543), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("35e8f22d-40e3-454f-8c82-af50e81227e5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7431), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PlannedDurationInMinutes", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "smallint", "PlannedDurationInMinutes", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7431), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("37ca1513-b100-4af7-a7dc-5535c9041b60"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8301), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8302), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("39609eae-9ca8-4a8d-93a5-52d28889e80b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8026), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDatePrecision", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "varchar", "EffectiveDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8026), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3a41011e-8561-47f7-9efd-f98a2b121c01"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8353), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8353), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3a6933ca-e959-4cb9-a5c7-7c85776cd2c3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8419), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AppointmentSlotGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "AppointmentSlotGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8419), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3c38425d-ca08-458e-8aa6-6dbde22d075f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8308), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DurationTerm", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "varchar", "DurationTerm", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8309), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3d1c757f-4c5c-41c4-9431-3d76f54405fb"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7394), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AppointmentDelayInMin", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "int", "AppointmentDelayInMin", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7395), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3f07700f-b59e-4e13-b8a6-e04872461ba3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7928), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Surname", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "varchar", "Surname", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7928), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3f51ce5d-7894-4332-9ce2-28f9bfffd9cc"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "StartDate", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "date", "StartDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7936), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("3fbc97b7-a830-4e59-a534-18e27c1659de"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8111), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8113), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("405afb0c-1252-4c8e-b1d7-193dc2ea61ae"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7624), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7625), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("40a02930-815a-4396-9893-7138128667d0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7957), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SessionCategoryDisplayName", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "varchar", "SessionCategoryDisplayName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7957), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("424e2ade-0b15-4014-bd24-0e66e50c69a5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralMode", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "varchar", "ReferralMode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7688), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("42c1ad57-365d-43c8-a746-503c9b723060"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7536), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DateOfBirth", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "date", "DateOfBirth", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7536), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("43d87e04-fddf-406d-9172-adffc4295be0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8047), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8048), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("4737f612-058e-4491-829f-a336fb9136e1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7799), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7799), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("479b24dc-8673-4a71-ac3b-2b45d1669614"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8081), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8081), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("47c4a842-a333-4034-9741-a8aea34e6159"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7573), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Village", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "Village", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7573), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("48e9e1a9-9233-495e-a5c5-c1398a433ad8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8330), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredTime", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "time", "EnteredTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8331), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("4a71dd62-bef0-4d7b-a967-4ae93751a69b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7524), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7525), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("4a79190f-0271-4e66-936f-8d4bfd10a5ba"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7418), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DnaReasonCodeId", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "bigint", "DnaReasonCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7419), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4affbb36-dbe5-483b-bdb9-3b21565e0658"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7710), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralSourceId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ReferralSourceId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7710), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4baf1d59-6c43-4d72-a1f9-318e44cf1bae"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7854), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EmisCodeCategoryDescription", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "EmisCodeCategoryDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7854), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4c55b3a2-b325-4835-939e-5e3ef835bf66"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7788), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ObservationType", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "varchar", "ObservationType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7789), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4c80e258-93f8-4631-b83b-d5a1254bbdca"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7835), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ProblemGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "ProblemGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7836), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4cdebe09-d9ab-4229-91e2-4ca9e6899089"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7950), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EndDate", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "date", "EndDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7950), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("4d556e6a-4037-4c29-a40d-3fe284679693"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7655), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7656), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("4e3e8c9e-58ed-4aab-acf8-b969e8d44cd5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7584), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CarerRelation", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "CarerRelation", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7584), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("4eb23b6a-a682-482d-b9c3-689f29d1856a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8181), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ProblemObservationGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "ProblemObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8182), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("4eb79c3b-df7a-4c59-9d97-ac930ca623e6"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8097), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ItemGuid", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "uniqueidentifier", "ItemGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8098), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("4ffb768c-34f2-4aea-ab97-497a3d5c9e32"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8155), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8155), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("50557d75-1849-4528-8b57-68a0eebb4877"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7907), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ContractEndDate", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "date", "ContractEndDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7907), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("50c22935-6a40-4b97-b7d5-e7acd32f7e19"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7459), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SessionGuid", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "uniqueidentifier", "SessionGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7459), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("52f25a39-d50f-4e0e-9dd5-729c7f9c3fee"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7728), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralEpisodeClosureDate", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "date", "ReferralEpisodeClosureDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7729), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("53dbbec5-311a-49f3-9dc7-143b9f150eab"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8471), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8472), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("54419442-e140-4642-ab30-af6e2dd74228"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7813), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7813), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("546f7053-f243-43f2-8479-c97165c1c97c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8517), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "County", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "County", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8518), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("556326f1-54e5-4c38-96ff-2a8782d03f59"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8290), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8291), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("559aef93-6209-456a-8b5c-d859e8c00e49"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8521), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EmailAddress", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "EmailAddress", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8521), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("56f2ea58-47f0-4229-8c22-b101d390bd7d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7971), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "AppointmentSessionGuid", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "uniqueidentifier", "AppointmentSessionGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7972), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("56faac0a-ccd0-4019-9581-108006412c95"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7895), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7895), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("5712c2a5-7e44-4c16-8e98-1d647331f00d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7503), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "UsualGpUserInRoleGuid", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "UsualGpUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7503), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("576c97af-fde1-408d-9042-b75448adc98d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8437), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8437), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("578e46f1-fb0b-4766-adb1-3204d0ee92c0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7939), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EndTime", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "time", "EndTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7940), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5870d3d6-c36d-444a-bd48-0ee20b73c8dc"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8132), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8132), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5ae2fff6-23ec-46cc-a10e-7dd3d0c5cd1c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8313), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsActive", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "bit", "IsActive", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8313), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5b3ef045-3abc-48e2-8198-c31018c99481"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7443), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientWaitInMin", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "int", "PatientWaitInMin", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7444), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5ddcf642-c337-471b-bccf-d7feebbd417e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7595), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7595), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("5dfa3bcf-5f87-4ae7-82af-f4467905e4c7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7462), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7463), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("61e24c11-d0d9-4e4c-9ba8-f44aab8bd573"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7609), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ExternalUsualGPGuid", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "ExternalUsualGPGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7610), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("62d21d9d-878c-4d7f-a814-feca5a481cee"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7975), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7975), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6307fe1e-287a-4e0d-b949-eb8696364f84"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7846), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ParentObservationGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "ParentObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7846), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6312a346-339e-43f1-bf4e-ab46a2669f80"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7576), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NHSNumberStatus", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "char", "NHSNumberStatus", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7577), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("635198f6-16c4-4033-a433-58cf962ffa6b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7546), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CarerName", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "CarerName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7547), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("63c6725f-d1d1-4349-8320-275874d68594"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8002), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredDate", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "date", "EnteredDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8003), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("63ce742c-4532-4211-9f8d-32ac44e1d557"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7943), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7943), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("63ef80ea-feb3-46e3-ae65-493ae81df639"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7521), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "MobilePhone", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "MobilePhone", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7521), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6416ef97-03b3-4a28-9fcf-195b92d5dc6e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8135), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EstimatedNhsCost", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "smallmoney", "EstimatedNhsCost", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8136), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("647e5d2d-589a-4caf-b777-2d3a8122cd2f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7703), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralTargetOrganisationGuid", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "uniqueidentifier", "ReferralTargetOrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7703), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("64e88c5b-31a0-4857-b96e-342b71572897"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8232), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EndDate", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "date", "EndDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8232), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("665021ae-e50e-4874-a502-7189380d5f53"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8117), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Term", "", "", "", null, null, new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"), "varchar", "Term", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8117), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6694a4bb-7729-43d0-8b66-e904d79cc2b3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8059), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Dosage", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "varchar", "Dosage", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8059), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("674ced77-2991-46c0-8e65-abe2fe966c03"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8327), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsComplete", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "bit", "IsComplete", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8328), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6828c5b8-50fe-41a2-8b69-6eaa22fd86f9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8252), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ParentProblemObservationGuid", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "uniqueidentifier", "ParentProblemObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8252), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("689f2c1c-6a26-4daa-aff2-2d223def8b52"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7698), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7699), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6a4af2a2-ed38-4e42-9e98-1d630f8c396d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8475), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "HouseNameFlatNumber", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "HouseNameFlatNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8475), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("6aacbf40-a3cc-4993-bb1a-3c92aa56366d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8402), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8403), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("6bc62b7a-c062-48a1-846a-127626b977f1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8067), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModifiedDate", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "date", "ModifiedDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8068), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6d5c31f4-56f5-4cf3-a875-ad137c8293d8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ExpectedDuration", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "smallint", "ExpectedDuration", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6e37a8c1-9a0d-4cce-a268-724bd1cd3d17"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7960), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LocationGuid", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "uniqueidentifier", "LocationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7961), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6f328079-ecd0-4b2e-8b37-e4f40f228f9a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7842), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumericRangeLow", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "float", "NumericRangeLow", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7843), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6f7779fe-1ecc-4da1-8647-51226936afc2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8022), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8023), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("6fac827e-b9bd-4ca2-90c4-6ec500a05e03"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7910), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "JobCategoryCode", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "char", "JobCategoryCode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7911), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("70de2594-ecea-41ea-8fac-b0302c2b5e65"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7968), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7968), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("70eb906b-851b-4278-9540-2badcc279e10"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8143), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8143), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("7168fb0c-1142-4924-b27d-d39831a75037"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7878), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReadTermId", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "ReadTermId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7879), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("716fab26-5b55-4d46-a4de-10873d0625b4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7769), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredByUserInRoleGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "EnteredByUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7770), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("71c34eec-9b5f-49c7-a3be-ace36b45f15a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8370), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8371), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("72b1c7a9-b6d5-40a7-a699-bf6598047882"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7828), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ObservationGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "ObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7828), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("73b108e5-328c-4160-95ba-c96bc45a73d9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7447), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SendInTime", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "time", "SendInTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7447), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("73fd24a5-dddd-4837-8c9e-027cec3175f9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7485), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "HomePhone", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "HomePhone", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7485), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("74324406-77e4-4f78-b486-5ce18ecce5e3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7550), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PersonGuid", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "uniqueidentifier", "PersonGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7550), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("74836307-2d5d-46b6-a350-7fdf1a9bcee2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8422), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredByUserInRoleGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "EnteredByUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8423), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("754b4547-831d-48f5-a97a-efdd0b4cadc8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8209), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredByUserInRoleGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "EnteredByUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8209), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("766fde1a-e0f4-4a06-a565-749f0e62a512"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7663), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ParentOrganisationGuid", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "uniqueidentifier", "ParentOrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7663), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("771145fc-579e-4452-b331-11329dafab55"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7691), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralReceivedTime", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "time", "ReferralReceivedTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7692), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("77517652-b41d-4954-bd95-6fa3554c03cd"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8410), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ConsultationGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "ConsultationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8411), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("778959a1-4baa-4f70-863f-92b4db0b1e01"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7795), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DocumentGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "DocumentGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7796), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7a3952d1-f745-4c48-ade4-a7a3483ac348"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7743), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralClosureReasonCodeId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ReferralClosureReasonCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7743), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7a52a66a-759d-4e7b-b0f7-d5a48c5fcba7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8457), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredTime", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "time", "EnteredTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8457), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7d064556-e6b3-410a-801e-8682b2bb75c6"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7558), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DateOfDeath", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "date", "DateOfDeath", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7558), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("7fef5aaf-0232-4907-88ad-3d58e10c15d5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8014), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumberOfIssues", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "tinyint", "NumberOfIssues", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8015), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("80d0ff9c-0d05-4727-a603-595dccd73f4b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8166), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CourseDurationInDays", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "int", "CourseDurationInDays", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8167), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("81062b69-2d5e-4ff3-abc8-8b70fab4cc38"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7736), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferringCareProfessionalStaffGroupCodeId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ReferringCareProfessionalStaffGroupCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7737), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("81423693-49b9-4554-a113-5db584f79a73"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7953), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Private", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "bit", "Private", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7954), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8170d54f-b6c8-4bbc-9763-92b374544edd"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8319), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AssociatedText", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "varchar", "AssociatedText", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8320), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("81b1e3f7-525e-4b24-8e4b-1342909ef6cd"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7499), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Surname", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "Surname", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7499), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8434011f-6ae2-419c-a77e-220dfb21c028"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8076), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "UserInRoleGuid", "", "", "", null, null, new Guid("97c6abcb-94ea-4ede-9f7d-1856d4c776ba"), "uniqueidentifier", "UserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8077), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("860c0937-8adf-4860-a7c7-5791aa8f7f19"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7784), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsConfidential", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "bit", "IsConfidential", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7785), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8617a57c-d899-4320-b1e2-88c4b7704072"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7865), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NationalCode", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "NationalCode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7865), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("86668055-7b54-4cd3-aa7d-bfc0dc6b223e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8445), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ClinicianUserInRoleGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "ClinicianUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8445), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("874eef90-39de-4367-a482-5f4d8c9d6f84"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8360), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8361), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("87af5a28-4975-4e91-8665-daabc185a403"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7888), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7888), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8bcd3442-31bd-40c6-a246-de59da0fe2ab"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8271), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8272), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8c0b89fe-11fb-4196-a246-17a17f4592d8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7645), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7646), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("8c5eb361-3fb3-478e-afa2-97970139dd69"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7918), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "UserInRoleGuid", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "uniqueidentifier", "UserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7918), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("8c9be888-fbcb-4394-bc9e-3e27571254fc"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8484), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Village", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "Village", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8485), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("924df98b-9eb0-4585-9376-87dbbda62e01"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7561), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "HouseNameFlatNumber", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "HouseNameFlatNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7562), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("93111020-404c-4712-84cf-8febfda90bda"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8367), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModifiedDate", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "date", "ModifiedDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8367), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9630a6a7-d51e-4912-82ec-3f9372dfbbd1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7718), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralEpisodeRTTMeasurementTypeId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ReferralEpisodeRTTMeasurementTypeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7718), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("969c634e-3f3a-419a-8526-e10edbd8c5ca"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8128), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DmdProductCodeId", "", "", "", null, null, new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"), "bigint", "DmdProductCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8128), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("97357fcd-5559-4a09-a7ac-901e3ef851bf"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7423), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DidNotAttend", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "bit", "DidNotAttend", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7423), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9836be8a-a6d8-40fa-968e-c228f89bb914"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8453), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8454), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("986291f1-06f6-40b0-81db-ca076898991c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8391), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "SessionGuid", "", "", "", null, null, new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"), "uniqueidentifier", "SessionGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8392), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("99016a1e-3bed-4399-bc4e-203e6fcd8a5a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8220), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ProblemStatusDescription", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "ProblemStatusDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8221), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9976bd33-0939-4751-8906-a4dfa5383329"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7806), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AssociatedText", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "varchar", "AssociatedText", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7806), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9b109e35-f3f4-4cc4-b260-531ae5baa6b9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8406), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ConsultationSourceCodeId", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "bigint", "ConsultationSourceCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8406), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9df91660-cf3c-41bf-bf99-2d4c95f1eb50"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7773), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ClinicianUserInRoleGuid", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "uniqueidentifier", "ClinicianUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7773), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9e620384-0990-4704-b3ee-1414ad4b5d8f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7466), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Disabled", "", "", "", null, null, new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "bit", "Disabled", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7466), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9eb802bb-91fd-4fbd-9d1b-e0a33f363e62"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7598), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumberAndStreet", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "NumberAndStreet", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7599), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("a037fb68-185f-404c-a68d-b5191fcde40f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8124), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("b3bffec5-2a51-438c-9259-5b5daf338384"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8125), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a0c8afb7-71e9-49ac-97e4-e30434d41bd4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7455), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7456), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("a377d9c8-b36c-4398-b94d-dffd2b9b7e2f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7839), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7839), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a4ecc6d6-aadd-4fdd-b2d8-211bffe8a60a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8283), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredDate", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "date", "EnteredDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8284), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a6cb4b9e-865b-4a10-8b27-fc6e18053946"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7617), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientNumber", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "int", "PatientNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7618), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a7ced9df-e3f3-421c-b52e-e851561612cd"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7602), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "County", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "County", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7603), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a7fc976d-e924-4b1a-80ba-25fa11c6eada"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8287), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDate", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "date", "EffectiveDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8287), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a8181d58-e6fb-4d84-9ba8-1d3426d63b25"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8217), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SignificanceDescription", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "SignificanceDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8217), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("a9d7e3a9-1d94-48bf-a6b6-f2b4ea642c6d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7606), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DummyType", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "bit", "DummyType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7606), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("abbd7ea3-5e9f-429b-9bc9-2eac05e365b0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7506), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "GivenName", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "GivenName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7507), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ac4fd120-5184-43d2-bcbc-3aa661029deb"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8235), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EndDatePrecision", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "varchar", "EndDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8236), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("acf5ce30-c463-45e1-9b36-0ae006ebdedf"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7765), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDate", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "date", "EffectiveDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7766), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ada316d2-d3bf-4f9e-a768-bc2485a08ada"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7821), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NumericUnit", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "varchar", "NumericUnit", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7822), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b18fb01b-496b-4c19-b834-ccfb9247fb16"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7635), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsMainLocation", "", "", "", null, null, new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "bit", "IsMainLocation", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7636), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b1f86138-bd5e-4078-911b-4d81c78e4414"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8298), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredByUserInRoleGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "EnteredByUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8298), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("b296bea3-8590-4c82-8060-87feed620d5f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8364), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "UserInRoleGuid", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "uniqueidentifier", "UserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8364), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("b482e424-abfd-46ab-8919-015fe2cede4e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8465), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8465), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("b5533594-0720-40fd-b3b2-52e3497b8b40"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8499), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "MainContactName", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "MainContactName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8499), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b586ac34-995f-4743-82f0-21f1f4e3d76d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7695), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralUrgency", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "varchar", "ReferralUrgency", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7695), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("b7c55d98-2ca5-498d-83c5-65821f69b0a9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8178), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "IssueRecordGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "IssueRecordGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8178), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("b81b6fa4-1d67-4843-8001-d1236c0c046e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7591), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Postcode", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "Postcode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7591), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b99ec1be-734e-4f3f-b1d2-52dffc9ce612"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8029), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "IsActive", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "bit", "IsActive", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8030), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ba5c3b79-507e-4afd-bb65-641c9d859763"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7659), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationType", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "varchar", "OrganisationType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7659), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("bc79b37f-d82d-4f32-8e1f-9834734f114e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8323), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8323), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("bce7924e-ffbb-4434-af88-e3b147b502a7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8341), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LocationTypeDescription", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "varchar", "LocationTypeDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8341), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("bd95782e-0020-4fa8-ac92-ff34a5438c2a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7621), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EmailAddress", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "EmailAddress", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7621), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("becfbad6-9da2-48e7-8841-82609bb8cac9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7792), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7792), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("bf4b78f7-863c-42d7-9465-fa11a6a9b175"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8460), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8461), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("bfc76c18-4434-4aae-924a-d8dd126e4014"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7924), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7925), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0cb789d-f745-4a0d-b166-763c499563c0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8415), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDate", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "date", "EffectiveDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8415), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c43440ae-99c5-4722-84f9-29dc752e81d5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8228), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8228), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c5763f60-c5de-42d1-b97e-d78745e281f1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8055), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8055), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("c6c1fe44-2147-41ca-8100-48c1179545a0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7517), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7518), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("c8aad664-4329-40df-bdae-404d16ad28e0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8488), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LocationName", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "LocationName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8488), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("c8b369a5-b9c1-48f7-9aeb-e56e39eea7ba"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8345), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "DiaryGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "DiaryGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8346), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("c95f54b9-743a-43c5-9336-753646b2bae0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7732), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralServiceType", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "varchar", "ReferralServiceType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7733), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("cceb2b1b-d848-41a2-b987-1c256c1caf3d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8139), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredTime", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "time", "EnteredTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8139), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("cede99f7-0fac-416c-aca0-359b1ceb6665"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7903), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "GivenName", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "varchar", "GivenName", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7904), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("cf34f086-c4f2-49c2-9b12-8cec89f76d73"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8388), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "UserInRoleGuid", "", "", "", null, null, new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"), "uniqueidentifier", "UserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8388), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("cf86d807-0488-4fdc-86cd-fbc1849c2232"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8395), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("d506dc20-115a-4bd0-88f0-da6decbea70a"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8396), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("cfc21067-9cb1-4e8f-bd06-698befaad209"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7405), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PatientGuid", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "uniqueidentifier", "PatientGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7406), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d019d451-4ef0-4a26-ac7c-3fe0040a03f8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7721), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7722), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d03dff77-9c74-49bb-a5a7-10f00a52eadf"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7899), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ContractStartDate", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "date", "ContractStartDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7899), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d04b3225-4455-4dbf-879c-52521881ba32"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7857), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NationalDescription", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "NationalDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7858), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d0eb085d-9415-4f52-aee2-3d2631ee17e2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7776), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7777), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d10f35d9-7528-41ed-8362-25a13e3d19c7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8224), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ObservationGuid", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "uniqueidentifier", "ObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8225), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("d1569f7a-17b0-4499-86bc-8da3f62d78df"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8255), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LastReviewUserInRoleGuid", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "uniqueidentifier", "LastReviewUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8257), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d21fb3f4-063a-464c-8732-c5a8aeca001d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7921), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Title", "", "", "", null, null, new Guid("699e9f7a-9ca1-482c-8c97-bb996f146ff3"), "varchar", "Title", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7921), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d28d60b7-a11f-420d-9b78-8978221a12b7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8426), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDatePrecision", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "varchar", "EffectiveDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8426), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d3c64bb3-2e70-4eac-b1fb-5bf4fdc374c9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7401), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AppointmentDate", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "date", "AppointmentDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7402), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d472d4c8-b215-4477-8f0d-687e8f698158"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8305), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8305), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d48e3d13-de43-429a-bbc3-0cdcc1ca8ea1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7642), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CDB", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "int", "CDB", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7643), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d502b3f2-936d-449b-90cd-a783fb2942f4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7631), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "LocationGuid", "", "", "", null, null, new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "uniqueidentifier", "LocationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7632), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("d57947e1-16c0-4b9d-9573-3d2767815451"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7713), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralEpisodeDischargeLetterIssuedDate", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "date", "ReferralEpisodeDischargeLetterIssuedDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7714), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d59b6e17-2905-46f7-977b-545cdbf9251e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7539), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ExternalUsualGP", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "ExternalUsualGP", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7540), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d5b77b5b-ace9-4c54-af3e-bae4449696c8"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7470), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7471), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("d7919a88-1042-4951-991d-be49aba34367"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7751), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralReasonCodeId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ReferralReasonCodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7751), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d7ab6445-47e1-4602-8c29-66d500b5ca51"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8242), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LastReviewDate", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "date", "LastReviewDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8243), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("d927c02d-5ebc-4b7b-9b7f-f7fab5ca3bc2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8280), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8280), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("d93ff951-cc6d-4c57-addf-cf34965074c9"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7762), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralEndDate", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "date", "ReferralEndDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7762), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d98bab5e-1b3d-4138-b97c-b71db86d30c0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7677), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "MainLocationGuid", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "uniqueidentifier", "MainLocationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7677), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("dbe48e80-b8d1-4140-b030-8a1c348ac3c6"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7666), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CloseDate", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "date", "CloseDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7667), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("de36b74c-6770-4a9a-8e53-3a032d3170af"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8399), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8399), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("deed3198-0bfa-4313-9919-64090878ff40"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8037), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "CodeId", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "bigint", "CodeId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8037), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e0b42e50-0db1-440a-b392-7056af38f492"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7861), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Term", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "varchar", "Term", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7861), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e0f45b74-6c7a-4c6f-b913-41d887443cd3"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7514), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "NhsNumber", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "char", "NhsNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7514), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e1a611db-e710-467c-9404-de8e82dfcdc7"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7474), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("07009820-23af-4ff1-a768-0cd90e22b0d4"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7474), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("e4866ef8-7a9f-49ea-af3d-2f2adc388255"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7706), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ReferralReceivedDate", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "date", "ReferralReceivedDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7706), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e49dcd0d-3820-4c45-9990-3b799d0b733b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8010), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ProblemObservationGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "ProblemObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8010), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e5be72d6-38eb-44c4-b087-6e1bc0120733"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7725), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ObservationGuid", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "uniqueidentifier", "ObservationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7725), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e6056baa-29fe-43b4-bc21-e0c8a89ec112"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8524), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LocationTypeDescription", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "LocationTypeDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8525), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e7274bb8-ab3c-4138-b091-9ddddef4499e"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7875), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "ParentCodeID", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "bigint", "ParentCodeID", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7875), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e7300f3a-c370-43d5-b268-44286a302c60"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8151), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ClinicianUserInRoleGuid", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "uniqueidentifier", "ClinicianUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8151), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e73c2a6a-ca1e-4149-b692-e8fb3cc67d48"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7639), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7639), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("e7fd9c3b-207a-426d-829c-5864087b0ceb"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8158), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "Dosage", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "varchar", "Dosage", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8159), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e88eef15-2e0c-42c2-a5f5-4cba09df8b7d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8495), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PhoneNumber", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "varchar", "PhoneNumber", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8495), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e964019e-39bf-4477-ba06-9cff9d3e44ae"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8433), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EnteredDate", "", "", "", null, null, new Guid("dfb6db14-012e-40eb-b8b2-26163db58e06"), "date", "EnteredDate", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8434), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("eae2509e-0b08-42c6-9123-205c725291a2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8481), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ParentLocationGuid", "", "", "", null, null, new Guid("e44e12a4-df37-401e-afc9-08024be3991a"), "uniqueidentifier", "ParentLocationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8481), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("eb447e65-d19d-491e-b4bf-a0517a64b79b"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7427), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "AppointmentStartTime", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "time", "AppointmentStartTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7427), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("eb5281e2-efc5-444c-a92d-1a2603721b4f"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7997), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "QuantityUnit", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "varchar", "QuantityUnit", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7997), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("eb7554df-f80e-4194-886d-bee5babdd62c"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7439), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "LeftTime", "", "", "", null, null, new Guid("01170c3b-f7ef-4c02-b7f0-a251a25f470b"), "time", "LeftTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7440), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("eba41453-c8c2-491d-bb86-9598de7ffab2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7569), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "MiddleNames", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "varchar", "MiddleNames", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7570), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsDeleteFlag", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("ec289217-c6b7-4837-8b8a-2cbc9fbc5df0"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7628), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "Deleted", "", "", "", null, null, new Guid("41073b65-0cfd-4ced-bd4d-75def2bb0977"), "bit", "Deleted", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7628), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("ed375f05-4451-42fa-bebe-96c194fcfddf"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8264), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "OrganisationGuid", "", "", "", null, null, new Guid("bd125c16-e020-49ba-8fd6-d1ce8201d5db"), "uniqueidentifier", "OrganisationGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8265), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ee736134-e8a9-4f6b-9057-aefe27a222d2"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7809), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "EffectiveDatePrecision", "", "", "", null, null, new Guid("4b4db138-a64e-47d0-83a0-68ec97b5ac8a"), "varchar", "EffectiveDatePrecision", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7810), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ef4b1a02-bf0c-4aee-bf72-0fb21a303bc1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7993), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "PrescriptionType", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "varchar", "PrescriptionType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7994), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("f30ae7c6-9f2d-42d9-9ba4-9a44a658fb6d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7964), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "StartTime", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "time", "StartTime", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7965), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("f318741a-31bf-41ad-9e30-627955fbcefe"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8170), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("b9ba8645-9307-4cf4-8336-519f0e685bf7"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8171), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("f48d909e-a7ac-4990-b8da-2bc87429e7a5"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8006), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ClinicianUserInRoleGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "ClinicianUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8006), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsBusinessKey", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("f48df6b4-ada9-4504-9a4a-ef68a0edaf8d"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8064), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", true, false, true, null, "", 0, "DrugRecordGuid", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "uniqueidentifier", "DrugRecordGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8064), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("f4f8d8c5-735c-42f3-b534-bf92030606d4"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7754), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("4a392c33-e268-4d4f-a474-9a5b6c803f12"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7754), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "IsVersionHashElement", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("f5c0457d-d3a6-4b87-931a-0ccdc4bdb343"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8356), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ModeType", "", "", "", null, null, new Guid("c89d4511-2697-4a1c-bb28-b50cf4f88a34"), "varchar", "ModeType", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8357), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("f715b6bb-91a0-452a-a589-0c4500aa3a73"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7492), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "DateofDeactivation", "", "", "", null, null, new Guid("28a1f2df-0def-46da-b319-a08d29b9c7b6"), "date", "DateofDeactivation", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7492), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("f7c81ff9-6224-42ea-8080-eed6479b334a"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7891), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SnomedCTConceptId", "", "", "", null, null, new Guid("5cec2ecc-4538-487c-8168-6052e06ae233"), "bigint", "SnomedCTConceptId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7892), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("f8924f7d-95dc-4e9f-b092-d0ad5046c1a1"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8337), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ClinicianUserInRoleGuid", "", "", "", null, null, new Guid("be4e6870-a802-4504-8b41-5ff3ad8ef74b"), "uniqueidentifier", "ClinicianUserInRoleGuid", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8338), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("f8d3db4c-f63f-4c2a-84bf-174411fe1e22"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7932), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "SessionTypeDescription", "", "", "", null, null, new Guid("93ba6ee2-33b9-4802-baa3-5379a45a3fa3"), "varchar", "SessionTypeDescription", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7932), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("fb03709a-af48-452d-b711-d5990db0fa38"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7680), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, true, null, "", 0, "ODSCode", "", "", "", null, null, new Guid("445d0610-0985-4f04-83aa-990ae7ec6ea8"), "varchar", "ODSCode", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(7681), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "ObjectColumns",
                columns: new[] { "Id", "CodeSystem", "ColumnDescription", "CreatedBy", "CreatedDate", "DataTypeId", "FhirDataType", "IsRelatedOrganisationId", "Length", "MaskingMethod", "OrdinalPosition", "OurColumnName", "PartitionColumnLevel", "PersonConfidentialDataType", "PopulatedBy", "Precision", "Scale", "SpecificationObjectId", "SqlDataType", "SupplierColumnName", "SupplierDateFormat", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("ff23c77a-22fa-4e27-86f6-a71cb4e43e36"), "", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8044), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000000-0000-0000-0000-000000000000"), "", false, null, "", 0, "ProcessingId", "", "", "", null, null, new Guid("94265244-abf3-4e54-a9e2-38e46f6ac485"), "int", "ProcessingId", "", "System", new DateTimeOffset(new DateTime(2023, 10, 16, 11, 33, 27, 858, DateTimeKind.Unspecified).AddTicks(8044), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_SupplierId",
                schema: "Configuration",
                table: "DataSets",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId",
                schema: "Configuration",
                table: "DataSetSpecifications",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_PresededById",
                schema: "Configuration",
                table: "DataSetSpecifications",
                column: "PresededById");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_SupersededById",
                schema: "Configuration",
                table: "DataSetSpecifications",
                column: "SupersededById");

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackingAudits_IngestionTrackingId",
                schema: "Ingestion",
                table: "IngestionTrackingAudits",
                column: "IngestionTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackings_FileName",
                schema: "Ingestion",
                table: "IngestionTrackings",
                column: "FileName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngestionTrackings_SupplierId",
                schema: "Ingestion",
                table: "IngestionTrackings",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectColumns_SpecificationObjectId",
                schema: "Configuration",
                table: "ObjectColumns",
                column: "SpecificationObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationObjects_DataSetSpecificationId",
                schema: "Configuration",
                table: "SpecificationObjects",
                column: "DataSetSpecificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Batch",
                schema: "LDS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "BatchHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "LDS")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "DataTypes",
                schema: "Configuration")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataTypesHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "IngestionTrackingAudits",
                schema: "Ingestion");

            migrationBuilder.DropTable(
                name: "ObjectColumns",
                schema: "Configuration")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ObjectColumnsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "OptOuts",
                schema: "Patient");

            migrationBuilder.DropTable(
                name: "Pds",
                schema: "Patient");

            migrationBuilder.DropTable(
                name: "IngestionTrackings",
                schema: "Ingestion");

            migrationBuilder.DropTable(
                name: "SpecificationObjects",
                schema: "Configuration")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SpecificationObjectsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "DataSetSpecifications",
                schema: "Configuration")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetSpecificationsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "DataSets",
                schema: "Configuration")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataSetsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "Configuration")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "Configuration");
        }
    }
}
