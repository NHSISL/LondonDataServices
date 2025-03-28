// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnReadOrInsertIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();

            var expectedException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                 service.RetrieveAllSpecificationObjectsAsync())
                     .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SpecificationObject> retrieveObjectsTask =
                this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                    randomSpecificationObject);

            SpecificationObjectProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingDependencyValidationException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnReadOrInsertIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();

            var expectedException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                 service.RetrieveAllSpecificationObjectsAsync())
                     .ThrowsAsync(dependencyException);

            // when
            ValueTask<SpecificationObject> retrieveObjectsTask =
                this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                    randomSpecificationObject);

            SpecificationObjectProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingDependencyException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnReadOrInsertIfServiceErrorOccursAsync()
        {
            // given
            SpecificationObject randomSpecificationObject = CreateRandomSpecificationObject();
            var serviceException = new Exception();

            var failedSpecificationObjectProcessingServiceException =
                new FailedSpecificationObjectProcessingServiceException(
                    message: "Failed SpecificationObject processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedException =
                new SpecificationObjectProcessingServiceException(
                    message: "SpecificationObject processing service error occurred, please contact support.",
                    innerException: failedSpecificationObjectProcessingServiceException);

            this.specificationObjectServiceMock.Setup(service =>
                 service.RetrieveAllSpecificationObjectsAsync())
                     .ThrowsAsync(serviceException);

            // when
            ValueTask<SpecificationObject> retrieveObjectsTask =
                this.specificationObjectProcessingService.ReadOrInsertSpecificationObjectAsync(
                    randomSpecificationObject);

            SpecificationObjectProcessingServiceException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingServiceException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjectsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
