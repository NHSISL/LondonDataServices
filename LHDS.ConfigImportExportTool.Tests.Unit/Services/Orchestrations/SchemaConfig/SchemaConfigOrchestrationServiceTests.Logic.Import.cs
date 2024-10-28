// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Bases.SchemaConfigs;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldImportSchemaConfigAsync()
        {
            // given
            List<SchemaConfig> randomSchemaConfigs = CreateRandomSchemaConfigs();
            List<SchemaConfig> inputSchemaConfigs = randomSchemaConfigs;
            List<SchemaConfig> expectedSchemaConfigs = inputSchemaConfigs;
            string randomDataSetName = GetRandomString();
            string inputDataSetName = randomDataSetName;

            foreach (SchemaConfig schemaConfig in inputSchemaConfigs)
            {
                ObjectColumn retrievedObjectColumn = new ObjectColumn
                {
                    SpecificationObjectId = schemaConfig.SpecificationObjectId,
                    SupplierColumnName = schemaConfig.SupplierColumnName,
                    OurColumnName = schemaConfig.OurColumnName,
                    ColumnDescription = schemaConfig.ColumnDescription,
                    OrdinalPosition = schemaConfig.OrdinalPosition,
                    PopulatedBy = schemaConfig.PopulatedBy,
                    FhirDataType = schemaConfig.FhirDataType,
                    SqlDataType = schemaConfig.SqlDataType,
                    Length = schemaConfig.Length,
                    Precision = schemaConfig.Precision,
                    Scale = schemaConfig.Scale,
                    SupplierDateFormat = schemaConfig.SupplierDateFormat,
                    IsWatermark = schemaConfig.IsWatermark,
                    IsSequencing = schemaConfig.IsSequencing,
                    IsBusinessKey = schemaConfig.IsBusinessKey,
                    IsUniqueRecordKey = schemaConfig.IsUniqueRecordKey,
                    IsVersionHashElement = schemaConfig.IsVersionHashElement,
                    IsSenderCode = schemaConfig.IsSenderCode,
                    IsAuthorCode = schemaConfig.IsAuthorCode,
                    IsRelatedOrganisationId = schemaConfig.IsRelatedOrganisationId,
                    IsDeleteFlag = schemaConfig.IsDeleteFlag,
                    IsSensitiveRecordMarker = schemaConfig.IsSensitiveRecordMarker,
                    IsPersonConfidentialData = schemaConfig.IsPersonConfidentialData,
                    PersonConfidentialDataType = schemaConfig.PersonConfidentialDataType,
                    MaskingMethod = schemaConfig.MaskingMethod,
                    CodeSystem = schemaConfig.CodeSystem,
                    PartitionColumnLevel = schemaConfig.PartitionColumnLevel,
                    DataTypeId = schemaConfig.DataTypeId,
                    IsForeignKey = schemaConfig.IsForeignKey,
                    ForeignKeyTableName = schemaConfig.ForeignKeyTableName,
                    ForeignKeyColumnName = schemaConfig.ForeignKeyColumnName
                };

                this.objectColumnProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertObjectColumnAsync(retrievedObjectColumn));

                SpecificationObject retrievedSpecificationObject = new SpecificationObject
                {
                    Id = schemaConfig.SpecificationObjectId,
                    DataSetSpecificationId = schemaConfig.DataSetSpecificationId,
                    SupplierObjectName = schemaConfig.SupplierObjectName,
                    OurObjectName = schemaConfig.OurObjectName,
                    ObjectDescription = schemaConfig.ObjectDescription,
                    InterchangeProtocol = schemaConfig.InterchangeProtocol,
                    IsPushedToUs = schemaConfig.IsPushedToUs,
                    IsPulledByUs = schemaConfig.IsPulledByUs,
                    DeletionHandling = schemaConfig.DeletionHandling,
                    IsSubmissionHeaderObject = schemaConfig.IsSubmissionHeaderObject,
                    IsTransactionLog = schemaConfig.IsTransactionLog,
                };

                this.specificationObjectProcessingServiceMock.Setup(service =>
                    service.ReadOrInsertSpecificationObjectAsync(retrievedSpecificationObject))
                        .ReturnsAsync(retrievedSpecificationObject);
            }

            // when
            await this.schemaConfigOrchestrationService.Import(inputSchemaConfigs, );

            // then
            foreach (SchemaConfig schemaConfig in inputSchemaConfigs)
            {
                this.objectColumnProcessingServiceMock.Verify(service =>
                    service.ReadOrInsertObjectColumnAsync(objectColumn),
                        Times.Once);

                this.specificationObjectProcessingServiceMock.Verify(service =>
                    service.ReadOrInsertSpecificationObjectAsync(i),
                        Times.Once);
            }

            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}