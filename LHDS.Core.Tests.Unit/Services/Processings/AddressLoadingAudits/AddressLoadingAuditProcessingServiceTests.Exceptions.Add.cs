// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Processings.AddressLoadingAudits.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = someAddressLoadingAudit;

            var expectedAddressLoadingAuditProcessingDependencyValidationException =
                new AddressLoadingAuditProcessingDependencyValidationException(
                    message: "Address loading audit processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressLoadingAuditServiceMock.Setup(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<AddressLoadingAudit> addressLoadingAuditAddTask =
                this.addressLoadingAuditProcessingService.AddAddressLoadingAuditAsync(inputAddressLoadingAudit);

            AddressLoadingAuditProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressLoadingAuditProcessingDependencyValidationException>(
                    addressLoadingAuditAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedAddressLoadingAuditProcessingDependencyValidationException);

            this.addressLoadingAuditServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressLoadingAuditProcessingDependencyValidationException))),
                         Times.Once);

            this.addressLoadingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = someAddressLoadingAudit;

            var expectedAddressLoadingAuditProcessingDependencyException =
                new AddressLoadingAuditProcessingDependencyException(
                    message: "Address loading audit processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.addressLoadingAuditServiceMock.Setup(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .Throws(dependencyException);

            // when
            ValueTask<AddressLoadingAudit> addressLoadingAuditAddTask =
                this.addressLoadingAuditProcessingService.AddAddressLoadingAuditAsync(inputAddressLoadingAudit);

            AddressLoadingAuditProcessingDependencyException actualException =
                await Assert.ThrowsAsync<AddressLoadingAuditProcessingDependencyException>(
                    addressLoadingAuditAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressLoadingAuditProcessingDependencyException);

            this.addressLoadingAuditServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressLoadingAuditProcessingDependencyException))),
                         Times.Once);

            this.addressLoadingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit someAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = someAddressLoadingAudit;

            var serviceException = new Exception();

            var failedAddressLoadingAuditProcessingServiceException =
                new FailedAddressLoadingAuditProcessingServiceException(
                    message: "Failed address loading audit processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedAddressLoadingAuditProcessingServiveException =
                new AddressLoadingAuditProcessingServiceException(
                    message: "Address loading audit processing service error occurred, contact support.",
                    innerException: failedAddressLoadingAuditProcessingServiceException);

            this.addressLoadingAuditServiceMock.Setup(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .Throws(serviceException);

            // when
            ValueTask<AddressLoadingAudit> addAddressLoadingTask =
                this.addressLoadingAuditProcessingService
                    .AddAddressLoadingAuditAsync(inputAddressLoadingAudit);

            AddressLoadingAuditProcessingServiceException actualException =
                await Assert.ThrowsAsync<AddressLoadingAuditProcessingServiceException>(addAddressLoadingTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressLoadingAuditProcessingServiveException);

            this.addressLoadingAuditServiceMock.Verify(service =>
                service.AddAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressLoadingAuditProcessingServiveException))),
                         Times.Once);

            this.addressLoadingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
