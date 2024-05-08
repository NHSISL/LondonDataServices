// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            ObjectColumn someObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = someObjectColumn;

            var expectedObjectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.objectColumnServiceMock.Setup(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<ObjectColumn> objectColumnAddTask =
                this.objectColumnProcessingService.ModifyObjectColumnAsync(inputObjectColumn);

            ObjectColumnProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyValidationException>(objectColumnAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyValidationException);

            this.objectColumnServiceMock.Verify(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn),
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            ObjectColumn someObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = someObjectColumn;

            var expectedObjectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.objectColumnServiceMock.Setup(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn))
                    .Throws(dependencyException);

            // when
            ValueTask<ObjectColumn> objectColumnAddTask =
                this.objectColumnProcessingService.ModifyObjectColumnAsync(inputObjectColumn);

            ObjectColumnProcessingDependencyException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingDependencyException>(objectColumnAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingDependencyException);

            this.objectColumnServiceMock.Verify(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedObjectColumnProcessingDependencyException))),
                         Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAsync()
        {
            // given
            ObjectColumn someObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = someObjectColumn;

            var serviceException = new Exception();

            var failedObjectColumnProcessingServiceException =
                new FailedObjectColumnProcessingServiceException(
                    message: "Failed ObjectColumn processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedObjectColumnProcessingServiveException =
                new ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: failedObjectColumnProcessingServiceException);

            this.objectColumnServiceMock.Setup(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn))
                    .Throws(serviceException);

            // when
            ValueTask<ObjectColumn> addObjectColumnTask =
                this.objectColumnProcessingService.ModifyObjectColumnAsync(inputObjectColumn);

            ObjectColumnProcessingServiceException actualException =
                await Assert.ThrowsAsync<ObjectColumnProcessingServiceException>(addObjectColumnTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedObjectColumnProcessingServiveException);

            this.objectColumnServiceMock.Verify(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn),
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
