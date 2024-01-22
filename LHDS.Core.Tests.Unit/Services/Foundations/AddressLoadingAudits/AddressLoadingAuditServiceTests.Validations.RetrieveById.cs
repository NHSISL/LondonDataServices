// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidAddressLoadingAuditId = Guid.Empty;

            var invalidAddressLoadingAuditException =
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            invalidAddressLoadingAuditException.AddData(
                key: nameof(AddressLoadingAudit.Id),
                values: "Id is required");

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: invalidAddressLoadingAuditException);

            // when
            ValueTask<AddressLoadingAudit> retrieveAddressLoadingAuditByIdTask =
                this.addressLoadingAuditService.RetrieveAddressLoadingAuditByIdAsync(invalidAddressLoadingAuditId);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    retrieveAddressLoadingAuditByIdTask.AsTask);

            // then
            actualAddressLoadingAuditValidationException.Should()
                .BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfAddressLoadingAuditIsNotFoundAndLogItAsync()
        {
            //given
            Guid someAddressLoadingAuditId = Guid.NewGuid();
            AddressLoadingAudit noAddressLoadingAudit = null;

            var notFoundAddressLoadingAuditException =
                new NotFoundAddressLoadingAuditException(someAddressLoadingAuditId);

            var expectedAddressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: notFoundAddressLoadingAuditException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noAddressLoadingAudit);

            //when
            ValueTask<AddressLoadingAudit> retrieveAddressLoadingAuditByIdTask =
                this.addressLoadingAuditService.RetrieveAddressLoadingAuditByIdAsync(someAddressLoadingAuditId);

            AddressLoadingAuditValidationException actualAddressLoadingAuditValidationException =
                await Assert.ThrowsAsync<AddressLoadingAuditValidationException>(
                    retrieveAddressLoadingAuditByIdTask.AsTask);

            //then
            actualAddressLoadingAuditValidationException.Should().BeEquivalentTo(expectedAddressLoadingAuditValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressLoadingAuditValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}