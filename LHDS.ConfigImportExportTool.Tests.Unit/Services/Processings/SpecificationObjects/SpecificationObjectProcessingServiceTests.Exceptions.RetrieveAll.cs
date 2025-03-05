// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjectsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<SpecificationObject>> retrieveObjectsTask =
                this.specificationObjectProcessingService.RetrieveAllSpecificationObjectsAsync();

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
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjectsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<SpecificationObject>> retrieveObjectsTask =
               this.specificationObjectProcessingService.RetrieveAllSpecificationObjectsAsync();

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
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

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
            ValueTask<IQueryable<SpecificationObject>> retrieveObjectsTask =
                this.specificationObjectProcessingService.RetrieveAllSpecificationObjectsAsync();

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
