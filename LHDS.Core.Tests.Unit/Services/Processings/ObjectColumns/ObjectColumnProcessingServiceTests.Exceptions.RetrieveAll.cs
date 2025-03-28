// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
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

            objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumnsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<ObjectColumn>> objectColumnRetrieveTask =
                objectColumnProcessingService.RetrieveAllObjectColumnsAsync();

            ObjectColumnProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyValidationException>(objectColumnRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyValidationException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyValidationException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedObjectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            objectColumnServiceMock.Setup(service =>
                service.RetrieveAllObjectColumnsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<ObjectColumn>> objectColumnRetrieveTask =
                objectColumnProcessingService.RetrieveAllObjectColumnsAsync();

            ObjectColumnProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyException>(
                    objectColumnRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
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
                service.RetrieveAllObjectColumnsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<ObjectColumn>> objectColumnRetrieveTask =
                objectColumnProcessingService.RetrieveAllObjectColumnsAsync();

            ObjectColumnProcessingServiceException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingServiceException>(
                    objectColumnRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingServiveException);

            objectColumnServiceMock.Verify(service =>
                service.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingServiveException))),
                         Times.Once);

            objectColumnServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
