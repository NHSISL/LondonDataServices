// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedObjectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(dependencyValidationException);

            // when
            Action objectColumnRetrieveAction = () =>
                objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingDependencyValidationException actualException =
                Assert.Throws<ObjectColumnProcessingDependencyValidationException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyValidationException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyValidationException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedObjectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(dependencyException);

            // when
            Action objectColumnRetrieveAction = () =>
                objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingDependencyException actualException =
                Assert.Throws<ObjectColumnProcessingDependencyException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedObjectColumnProcessingServiceException =
                new FailedObjectColumnProcessingServiceException(
                    message: "Failed ObjectColumn processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedObjectColumnProcessingServiveException =
                new ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: failedObjectColumnProcessingServiceException);

            objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumns())
                    .Throws(serviceException);

            // when
            Action objectColumnRetrieveAction = () =>
                objectColumnProcessingService.RetrieveAllObjectColumns();

            ObjectColumnProcessingServiceException actualException =
                Assert.Throws<ObjectColumnProcessingServiceException>(objectColumnRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingServiveException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumns(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingServiveException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
