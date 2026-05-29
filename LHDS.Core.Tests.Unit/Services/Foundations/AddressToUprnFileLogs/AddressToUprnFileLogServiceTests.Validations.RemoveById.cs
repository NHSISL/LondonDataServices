// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidId = Guid.Empty;

            var invalidAddressToUprnFileLogException =
                new InvalidAddressToUprnFileLogException(
                    message: "Invalid address to UPRN file log. Please correct the errors and try again.");

            invalidAddressToUprnFileLogException.AddData(
                key: nameof(AddressToUprnFileLog.Id),
                values: "Id is required");

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: invalidAddressToUprnFileLogException);

            // when
            ValueTask<AddressToUprnFileLog> removeByIdTask =
                this.addressToUprnFileLogService.RemoveAddressToUprnFileLogByIdAsync(invalidId);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(removeByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveByIdIfNotFoundAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            AddressToUprnFileLog noAddressToUprnFileLog = null;

            var notFoundAddressToUprnFileLogException =
                new NotFoundAddressToUprnFileLogException(someId);

            var expectedAddressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: notFoundAddressToUprnFileLogException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noAddressToUprnFileLog);

            // when
            ValueTask<AddressToUprnFileLog> removeByIdTask =
                this.addressToUprnFileLogService.RemoveAddressToUprnFileLogByIdAsync(someId);

            AddressToUprnFileLogValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnFileLogValidationException>(removeByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnFileLogValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressToUprnFileLogByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnFileLogValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressToUprnFileLogAsync(It.IsAny<AddressToUprnFileLog>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
