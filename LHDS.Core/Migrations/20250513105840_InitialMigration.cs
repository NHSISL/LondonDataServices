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
            migrationBuilder.EnsureSchema(
                name: "Addresses");

            migrationBuilder.EnsureSchema(
                name: "Audit");

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
                schema: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UPRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UPSN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    USRN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
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
                name: "Audits",
                schema: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AuditType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTypes",
                schema: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
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
                    UniqueReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                schema: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueReference = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchReference = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnstructuredPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    PostCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    AddressFormatQuality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostCodeQuality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MatchedWithAssign = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Qualifier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Algorithm = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MatchPattern = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: false),
                    IsExported = table.Column<bool>(type: "bit", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsIngestionTracked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CanDecryptIngestionTracking = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                    Version = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsCore = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDownloaded = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsError = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataSetName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DataSetAliases = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DataSetAuthor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SpecifiedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsNationallySpecified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CollectedBy = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsNationallyCollected = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataSourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ActiveTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
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
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SourceFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchReadyFolderPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSetSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncryptedFileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DecryptedFileName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Decrypted = table.Column<bool>(type: "bit", nullable: false),
                    IsDownloaded = table.Column<bool>(type: "bit", nullable: false),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    LastAttempt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastSeen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FileDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RecordCount = table.Column<long>(type: "bigint", nullable: false),
                    EncryptedFileSize = table.Column<long>(type: "bigint", nullable: false),
                    EncryptedFileSha256Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DecryptedFileSize = table.Column<long>(type: "bigint", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierSpecificationVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OurSpecificationVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMultiAuthorPerBatch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EntityChangeSynchronisation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateReleased = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateImplemented = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateSuperseded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SupersededById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PresededById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
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
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataSetSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierObjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OurObjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ObjectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InterchangeProtocol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsPushedToUs = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPulledByUs = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletionHandling = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsSubmissionHeaderObject = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsTransactionLog = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsCdmMasked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsCdmPcd = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FileFormatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificationObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierColumnName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OurColumnName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ColumnDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrdinalPosition = table.Column<int>(type: "int", nullable: false),
                    PopulatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FhirDataType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SqlDataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Length = table.Column<int>(type: "int", nullable: true),
                    Precision = table.Column<int>(type: "int", nullable: true),
                    Scale = table.Column<int>(type: "int", nullable: true),
                    SupplierDateFormat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsWatermark = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSequencing = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsBusinessKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsUniqueRecordKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsVersionHashElement = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSenderCode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsAuthorCode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRelatedOrganisationId = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleteFlag = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSensitiveRecordMarker = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPersonConfidentialData = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PersonConfidentialDataType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaskingMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodeSystem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PartitionColumnLevel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DataTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsForeignKey = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ForeignKeyTableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignKeyColumnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCaseSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleteCondition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsPostcode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsNumeric = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Address_PostCode",
                schema: "Addresses",
                table: "Address",
                column: "PostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UPRN",
                schema: "Addresses",
                table: "Address",
                column: "UPRN",
                unique: true,
                filter: "[UPRN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_AuditType",
                schema: "Audit",
                table: "Audits",
                column: "AuditType");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_CorrelationId",
                schema: "Audit",
                table: "Audits",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Audits_LogLevel",
                schema: "Audit",
                table: "Audits",
                column: "LogLevel");

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_DataSetName",
                schema: "Configuration",
                table: "DataSets",
                column: "DataSetName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSets_SupplierId",
                schema: "Configuration",
                table: "DataSets",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId_OurSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "DataSetId", "OurSpecificationVersion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataSetSpecifications_DataSetId_SupplierSpecificationVersion",
                schema: "Configuration",
                table: "DataSetSpecifications",
                columns: new[] { "DataSetId", "SupplierSpecificationVersion" },
                unique: true);

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
                name: "IX_Suppliers_Name",
                schema: "Configuration",
                table: "Suppliers",
                column: "Name",
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
                schema: "Addresses");

            migrationBuilder.DropTable(
                name: "Audits",
                schema: "Audit");

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
                schema: "Addresses");

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
