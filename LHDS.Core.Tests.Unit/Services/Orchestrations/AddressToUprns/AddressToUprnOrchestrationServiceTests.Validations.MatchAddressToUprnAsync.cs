// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchAddressToUprnIfInputStreamIsNullAndLogItAsync()
        {
            // given
            Stream invalidInput = null;
            string randomFileName = GetRandomString();
            Guid randomCorrelationId = GetRandomGuid();

            var invalidArgumentAddressToUprnOrchestrationException =
                new InvalidArgumentAddressToUprnOrchestrationException(
                    "Invalid address to UPRN orchestration argument(s), please correct the errors and try again.");

            invalidArgumentAddressToUprnOrchestrationException.UpsertDataList(
                key: "input",
                value: "Stream is required");

            var expectedAddressToUprnOrchestrationValidationException =
                new AddressToUprnOrchestrationValidationException(
                    message: "Address to UPRN orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressToUprnOrchestrationException);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: invalidInput,
                fileName: randomFileName,
                correlationId: randomCorrelationId);

            AddressToUprnOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationValidationException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnOrchestrationValidationException))),
                        Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnMatchAddressToUprnIfFileNameIsInvalidAndLogItAsync(
            string invalidFileName)
        {
            // given
            Stream randomStream = CreateSingleAddressCsvStream();
            Guid randomCorrelationId = GetRandomGuid();

            var invalidArgumentAddressToUprnOrchestrationException =
                new InvalidArgumentAddressToUprnOrchestrationException(
                    "Invalid address to UPRN orchestration argument(s), please correct the errors and try again.");

            invalidArgumentAddressToUprnOrchestrationException.UpsertDataList(
                key: "fileName",
                value: "Text is required");

            var expectedAddressToUprnOrchestrationValidationException =
                new AddressToUprnOrchestrationValidationException(
                    message: "Address to UPRN orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressToUprnOrchestrationException);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: randomStream,
                fileName: invalidFileName,
                correlationId: randomCorrelationId);

            AddressToUprnOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationValidationException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnOrchestrationValidationException))),
                        Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchAddressToUprnIfCorrelationIdIsEmptyAndLogItAsync()
        {
            // given
            Stream randomStream = CreateSingleAddressCsvStream();
            string randomFileName = GetRandomString();
            Guid emptyCorrelationId = Guid.Empty;

            var invalidArgumentAddressToUprnOrchestrationException =
                new InvalidArgumentAddressToUprnOrchestrationException(
                    "Invalid address to UPRN orchestration argument(s), please correct the errors and try again.");

            invalidArgumentAddressToUprnOrchestrationException.UpsertDataList(
                key: "correlationId",
                value: "Id is required");

            var expectedAddressToUprnOrchestrationValidationException =
                new AddressToUprnOrchestrationValidationException(
                    message: "Address to UPRN orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressToUprnOrchestrationException);

            // when
            ValueTask matchTask = this.addressToUprnOrchestrationService.MatchAddressToUprnAsync(
                input: randomStream,
                fileName: randomFileName,
                correlationId: emptyCorrelationId);

            AddressToUprnOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<AddressToUprnOrchestrationValidationException>(matchTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressToUprnOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAddressToUprnOrchestrationValidationException))),
                        Times.Once);

            this.assignServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.addressToUprnFileLogServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
