// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LHDS.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UPRN");

            migrationBuilder.EnsureSchema(
                name: "Configuration");

            migrationBuilder.EnsureSchema(
                name: "Ingestion");

            migrationBuilder.EnsureSchema(
                name: "Patient");

            migrationBuilder.EnsureSchema(
                name: "Configurations");

            migrationBuilder.EnsureSchema(
                name: "Ontology");

            migrationBuilder.EnsureSchema(
                name: "Terminology");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UPRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UPSN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    OrganisationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DepartmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SubBuildingName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuildingName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DependentThoroughfare = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Thoroughfare = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DoubleDependentLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DependentLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostTown = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressExtractionAudit",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressExtractionAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressLoadingAudit",
                schema: "UPRN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressLoadingAudit", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "SubscriberAgreements",
                schema: "Configurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierSharingAgreementShortName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SupplierSharingAgreementGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FtpUserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FtpPublicKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GpgPublicKey = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastPollStartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastPollEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriberAgreements", x => x.Id);
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
                    CanRelandIngestionTracking = table.Column<bool>(type: "bit", nullable: false),
                    CanDecryptIngestionTracking = table.Column<bool>(type: "bit", nullable: false),
                    CanDownloadIngestionTracking = table.Column<bool>(type: "bit", nullable: false),
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
                name: "TerminologyArtifacts",
                schema: "Ontology",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsCore = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDownloaded = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminologyArtifacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminologyPolls",
                schema: "Terminology",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastPoll = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminologyPolls", x => x.Id);
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    EncryptedFileSha256Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DecryptedFileSize = table.Column<int>(type: "int", nullable: false),
                    DecryptedFileSha256Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                columns: new[] { "Id", "CanDecryptIngestionTracking", "CanDownloadIngestionTracking", "CreatedBy", "CreatedDate", "Description", "FriendlyName", "Name", "UpdatedBy", "UpdatedDate", "CanRelandIngestionTracking" },
                values: new object[] { new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), false, false, "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Emis Supplier", "EMIS", "EMIS", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSets",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CollectedBy", "CreatedBy", "CreatedDate", "DataSetAliases", "DataSetAuthor", "DataSetName", "DataSourceType", "IsActive", "SpecifiedBy", "SupplierId", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EMIS", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PrimaryCareEMISDEV", "EMISDEV", "PrimaryCareEMISDEV", "PrimaryCareEMISDEV", true, "EMIS", new Guid("67680f17-9d0c-4474-8b35-56ca8f9df1f6"), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "Id", "ActiveFrom", "ActiveTo", "CreatedBy", "CreatedDate", "DataSetId", "DateImplemented", "DateReleased", "DateSuperseded", "EntityChangeSynchronisation", "IsActive", "IsMultiAuthorPerBatch", "IsPublished", "Notes", "OurSpecificationVersion", "PresededById", "SupersededById", "SupplierSpecificationVersion", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e8ebce80-e619-40ca-b45f-9c3ac0328143"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2123, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("6a62313a-7442-462e-b6e8-dec541ddd0ba"), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "", true, true, true, "This is a test dataset specification", "1.0", null, null, "7.0", "System", new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

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

            migrationBuilder.CreateIndex(
                name: "IX_SubscriberAgreements_SupplierSharingAgreementShortName",
                schema: "Configurations",
                table: "SubscriberAgreements",
                column: "SupplierSharingAgreementShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminologyArtifacts_FullUrl",
                schema: "Ontology",
                table: "TerminologyArtifacts",
                column: "FullUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminologyPolls_ResourceType",
                schema: "Terminology",
                table: "TerminologyPolls",
                column: "ResourceType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "UPRN");

            migrationBuilder.DropTable(
                name: "AddressExtractionAudit",
                schema: "UPRN");

            migrationBuilder.DropTable(
                name: "AddressLoadingAudit",
                schema: "UPRN");

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
                name: "ResolvedAddress",
                schema: "UPRN");

            migrationBuilder.DropTable(
                name: "SubscriberAgreements",
                schema: "Configurations");

            migrationBuilder.DropTable(
                name: "TerminologyArtifacts",
                schema: "Ontology");

            migrationBuilder.DropTable(
                name: "TerminologyPolls",
                schema: "Terminology");

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
