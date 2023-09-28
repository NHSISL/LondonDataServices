// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedObjectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(dependencyValidationException);

            // when
            Action objectColumnRetrieveAction = () =>
                this.objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingDependencyValidationException actualException =
                Assert.Throws<ObjectColumnProcessingDependencyValidationException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyValidationException);

            this.objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyValidationException))),
                         Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedObjectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(dependencyException);

            // when
            Action objectColumnRetrieveAction = () =>
                this.objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingDependencyException actualException =
                Assert.Throws<ObjectColumnProcessingDependencyException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyException);

            this.objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyException))),
                         Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedObjectColumnProcessingServiceException =
                new FailedObjectColumnProcessingServiceException(
                    message: "Failed ObjectColumn processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedObjectColumnProcessingServiveException =
                new ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, contact support.",
                    innerException: failedObjectColumnProcessingServiceException);

            this.objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(serviceException);

            // when
            Action objectColumnRetrieveAction = () =>
                this.objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingServiceException actualException =
                Assert.Throws<ObjectColumnProcessingServiceException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingServiveException);

            this.objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingServiveException))),
                         Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
