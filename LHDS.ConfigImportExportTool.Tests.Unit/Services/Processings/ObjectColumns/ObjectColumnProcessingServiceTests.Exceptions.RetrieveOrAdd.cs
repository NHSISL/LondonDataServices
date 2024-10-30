// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnReadOrInsertIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();

            var expectedException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ObjectColumnServiceMock.Setup(service =>
                 service.RetrieveAllObjectColumnsAsync())
                     .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<ObjectColumn> retrieveObjectsTask =
                this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(
                    randomObjectColumn);

            ObjectColumnProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyValidationException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.ObjectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnReadOrInsertIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();

            var expectedException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ObjectColumnServiceMock.Setup(service =>
                 service.RetrieveAllObjectColumnsAsync())
                     .ThrowsAsync(dependencyException);

            // when
            ValueTask<ObjectColumn> retrieveObjectsTask =
                this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(
                    randomObjectColumn);

            ObjectColumnProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.ObjectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnReadOrInsertIfServiceErrorOccursAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            var serviceException = new Exception();

            var failedObjectColumnProcessingServiceException =
                new FailedObjectColumnProcessingServiceException(
                    message: "Failed ObjectColumn processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedException =
                new ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: failedObjectColumnProcessingServiceException);

            this.ObjectColumnServiceMock.Setup(service =>
                 service.RetrieveAllObjectColumnsAsync())
                     .ThrowsAsync(serviceException);

            // when
            ValueTask<ObjectColumn> retrieveObjectsTask =
                this.ObjectColumnProcessingService.ReadOrInsertObjectColumnAsync(
                    randomObjectColumn);

            ObjectColumnProcessingServiceException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingServiceException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.ObjectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.ObjectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
