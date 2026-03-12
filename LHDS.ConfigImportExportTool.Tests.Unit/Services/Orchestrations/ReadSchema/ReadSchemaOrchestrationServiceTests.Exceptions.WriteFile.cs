// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema;
using LHDS.ConfigImportExportTool.Models.Orchestrations.ReadSchema.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Orchestrations.ReadSchema
{
    public partial class ReadSchemaOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(ReadSchemaOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnWriteFileIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputPath = GetRandomString();
            string inputTableName = GetRandomString();
            List<ObjectColumn> objectColumns = CreateRandomObjectColumns(isExport: true);

            List<SpecificationObject> inputSpecificationObjects =
                CreateRandomSpecificationObjects(objectColumns, inputTableName, true);

            var expectedDependencyException =
                new ReadSchemaOrchestrationDependencyValidationException(
                    message: "Read schema orchestration dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.csvHelperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask writeSchemaFileTask =
                this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, inputPath);

            ReadSchemaOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationDependencyValidationException>(
                    writeSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ReadSchemaOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowExceptionOnWriteFileIfDependencyExceptionOccursAndLogItAsync(
                Xeption dependencyException)
        {
            // given
            string inputPath = GetRandomString();
            string inputTableName = GetRandomString();
            List<ObjectColumn> objectColumns = CreateRandomObjectColumns(isExport: true);

            List<SpecificationObject> inputSpecificationObjects =
                CreateRandomSpecificationObjects(objectColumns, inputTableName, isExport: true);

            var expectedDependencyException =
                new ReadSchemaOrchestrationDependencyException(
                    message: "Read schema orchestration dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.csvHelperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(dependencyException);

            // when
            ValueTask writeSchemaFileTask =
                this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, inputPath);

            ReadSchemaOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationDependencyException>(
                    writeSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnWriteSchemaFileIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string inputPath = GetRandomString();
            string inputTableName = GetRandomString();
            List<ObjectColumn> objectColumns = CreateRandomObjectColumns(isExport: true);
            var serviceException = new Exception();

            List<SpecificationObject> inputSpecificationObjects =
                CreateRandomSpecificationObjects(objectColumns, inputTableName, isExport: true);

            var failedReadSchemaOrchestrationServiceException =
                new FailedReadSchemaOrchestrationServiceException(
                    message: "Failed read schema orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedServiceException =
                new ReadSchemaOrchestrationServiceException(
                    message: "Read schema orchestration service error occurred, " +
                        "please contact support.",
                    innerException: failedReadSchemaOrchestrationServiceException);

            this.csvHelperServiceMock.Setup(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()))
                        .ThrowsAsync(serviceException);

            // when
            ValueTask writeSchemaFileTask =
                this.readSchemaOrchestrationService.WriteFile(inputSpecificationObjects, inputPath);

            ReadSchemaOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationServiceException>(
                    writeSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.csvHelperServiceMock.Verify(service =>
                service.MapObjectToCsvAsync<CannonicalSchemaItem>(
                    It.IsAny<List<CannonicalSchemaItem>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Dictionary<string, int>>(),
                    It.IsAny<bool>()),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedServiceException))),
                       Times.Once);

            this.csvHelperServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.fileServiceMock.VerifyNoOtherCalls();
        }
    }
}