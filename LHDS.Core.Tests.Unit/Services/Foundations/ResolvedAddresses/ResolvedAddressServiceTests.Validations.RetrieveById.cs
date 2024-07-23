// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidResolvedAddressId = Guid.Empty;

            var invalidResolvedAddressException =
                new InvalidResolvedAddressException(
                    message: "Invalid resolved address. Please correct the errors and try again.");

            invalidResolvedAddressException.AddData(
                key: nameof(ResolvedAddress.Id),
                values: "Id is required");

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: invalidResolvedAddressException);

            // when
            ValueTask<ResolvedAddress> retrieveResolvedAddressByIdTask =
                this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(invalidResolvedAddressId);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    retrieveResolvedAddressByIdTask.AsTask);

            // then
            actualResolvedAddressValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfResolvedAddressIsNotFoundAndLogItAsync()
        {
            //given
            Guid someResolvedAddressId = Guid.NewGuid();
            ResolvedAddress noResolvedAddress = null;

            var notFoundResolvedAddressException =
                new NotFoundResolvedAddressException(someResolvedAddressId);

            var expectedResolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: notFoundResolvedAddressException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noResolvedAddress);

            //when
            ValueTask<ResolvedAddress> retrieveResolvedAddressByIdTask =
                this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(someResolvedAddressId);

            ResolvedAddressValidationException actualResolvedAddressValidationException =
                await Assert.ThrowsAsync<ResolvedAddressValidationException>(
                    retrieveResolvedAddressByIdTask.AsTask);

            //then
            actualResolvedAddressValidationException.Should().BeEquivalentTo(expectedResolvedAddressValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedResolvedAddressValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}