// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.SchemaConfigs.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.SchemaConfigs
{
    public partial class SchemaConfigOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(SchemaConfigOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnImportIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            Guid inputDataSetId = Guid.NewGuid();
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                specificationObject.ObjectColumns = newObjectColumns;
                inputSpecificationObjects.Add(specificationObject);
            }

            var expectedDependencyException =
                new SchemaConfigOrchestrationDependencyValidationException(
                    message: "Schema config orchestration dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask processSchemaFileTask =
                this.schemaConfigOrchestrationService.Import(inputSpecificationObjects, inputDataSetName, inputVersion);

            SchemaConfigOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<SchemaConfigOrchestrationDependencyValidationException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.dataSetProcessingServiceMock.Verify(service =>
             service.RetrieveAllDataSetsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SchemaConfigOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnImportIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            Guid inputDataSetId = Guid.NewGuid();
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();

            foreach (SpecificationObject specificationObject in randomSpecificationObjects)
            {
                List<ObjectColumn> newObjectColumns = CreateRandomObjectColumns(specificationObject.Id);
                specificationObject.ObjectColumns = newObjectColumns;
                inputSpecificationObjects.Add(specificationObject);
            }

            var expectedDependencyException =
                new SchemaConfigOrchestrationDependencyException(
                    message: "Schema config orchestration dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask processSchemaFileTask =
                this.schemaConfigOrchestrationService.Import(inputSpecificationObjects, inputDataSetName, inputVersion);

            SchemaConfigOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<SchemaConfigOrchestrationDependencyException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.dataSetProcessingServiceMock.Verify(service =>
             service.RetrieveAllDataSetsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnImpoortIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string inputDataSetName = GetRandomString(10);
            string inputVersion = GetRandomString(10);
            Guid inputDataSetId = Guid.NewGuid();
            List<SpecificationObject> randomSpecificationObjects = CreateRandomSpecificationObjects();
            List<SpecificationObject> inputSpecificationObjects = new List<SpecificationObject>();
            var serviceException = new Exception();

            var failedSchemaConfigOrchestrationServiceException =
                new FailedSchemaConfigOrchestrationServiceException(
                    message: "Failed schema config orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedServiceException =
                new SchemaConfigOrchestrationServiceException(
                    message: "Schema config orchestration service error occurred, " +
                        "please contact support.",
                    innerException: failedSchemaConfigOrchestrationServiceException);

            this.dataSetProcessingServiceMock.Setup(service =>
                service.RetrieveAllDataSetsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask processSchemaFileTask =
                this.schemaConfigOrchestrationService.Import(inputSpecificationObjects, inputDataSetName, inputVersion);

            SchemaConfigOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<SchemaConfigOrchestrationServiceException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.dataSetProcessingServiceMock.Verify(service =>
             service.RetrieveAllDataSetsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedServiceException))),
                       Times.Once);

            this.dataSetProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.specificationObjectProcessingServiceMock.VerifyNoOtherCalls();
            this.objectColumnProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}