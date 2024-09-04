// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjects())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveObjectsTask =
                this.specificationObjectProcessingService.RetrieveSpecificationObjectsByDataSetSpecificationId(someId);

            SpecificationObjectProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingDependencyValidationException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjects(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.specificationObjectServiceMock.Setup(service =>
                service.RetrieveAllSpecificationObjects())
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveObjectsTask =
                this.specificationObjectProcessingService
                    .RetrieveSpecificationObjectsByDataSetSpecificationId(someId);

            SpecificationObjectProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingDependencyException>(
                    retrieveObjectsTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjects(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAsync()
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
                service.RetrieveAllSpecificationObjects())
                    .Throws(serviceException);

            // when
            ValueTask<List<string>> addSpecificationObjectTask =
                this.specificationObjectProcessingService
                    .RetrieveSpecificationObjectsByDataSetSpecificationId(someId);

            SpecificationObjectProcessingServiceException actualException =
                await Assert.ThrowsAsync<SpecificationObjectProcessingServiceException>(
                    addSpecificationObjectTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedException);

            this.specificationObjectServiceMock.Verify(service =>
                service.RetrieveAllSpecificationObjects(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedException))),
                         Times.Once);

            this.specificationObjectServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
