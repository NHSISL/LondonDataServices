// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
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
            ShouldThrowDependencyValidationOnProcessFileIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string inputPath = GetRandomString();
            
            var expectedDependencyException =
                new ReadSchemaOrchestrationDependencyValidationException(
                    message: "Read schema orchestration dependency validation error occurred, " +
                        "please contact support.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<SpecificationObject>> processSchemaFileTask =
                this.readSchemaOrchestrationService.ReadFile(inputPath);

            ReadSchemaOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationDependencyValidationException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileServiceMock.Verify(service =>
             service.ReadFromFileAsync(It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ReadSchemaOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnProcessSchemaFileIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string inputPath = GetRandomString();

            var expectedDependencyException =
                new ReadSchemaOrchestrationDependencyException(
                    message: "Read schema orchestration dependency error occurred, " +
                        "please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(It.IsAny<string>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<SpecificationObject>> processSchemaFileTask =
                this.readSchemaOrchestrationService.ReadFile(inputPath);

            ReadSchemaOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationDependencyException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.fileServiceMock.Verify(service =>
             service.ReadFromFileAsync(It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnProcessSchemaFileIfServiceErrorOccursAndLogItAsync()
        {
            //given
            string inputPath = GetRandomString();
            var serviceException = new Exception();

            var failedReadSchemaOrchestrationServiceException =
                new FailedReadSchemaOrchestrationServiceException(
                    message: "Failed read schema orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedServiceException =
                new ReadSchemaOrchestrationServiceException(
                    message: "Read schema orchestration service error occurred, " +
                        "please contact support.",
                    innerException: failedReadSchemaOrchestrationServiceException);

            this.fileServiceMock.Setup(service =>
                service.ReadFromFileAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<SpecificationObject>> processSchemaFileTask =
                this.readSchemaOrchestrationService.ReadFile(inputPath);

            ReadSchemaOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ReadSchemaOrchestrationServiceException>(
                    processSchemaFileTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedServiceException);

            this.fileServiceMock.Verify(service =>
             service.ReadFromFileAsync(It.IsAny<string>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedServiceException))),
                       Times.Once);

            this.fileServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperServiceMock.VerifyNoOtherCalls();
        }
    }
}