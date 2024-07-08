// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact(Skip = "no longer used, will refactor out")]
        public async Task ShouldThrowValidationExceptionsOnMatchAndPersistResolvedAddressAsyncIfResolvedAddressIsNullAndLogItAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = null;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "no longer used, will refactor out")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnProcessIfPostCodeIsNullAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            randomResolvedAddress.PostCode = invalidText;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "postCode",
                values: "Text is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "no longer used, will refactor out")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnProcessIfJsonPostalAddressIsNullAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            randomResolvedAddress.JsonPostalAddress = invalidText;

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "jsonPostalAddress",
                values: "Text is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory(Skip = "no longer used, will refactor out")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionsOnProcessIfExtractedPostcodeIsNullAndLogItAsync(string invalidText)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            string postCode = invalidText;

            this.addressMatcherProcessingServiceMock.SetupSequence(processing =>
                processing.ExtractPostCode(randomResolvedAddress.JsonPostalAddress))
                    .Returns(postCode);

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "postCode",
                values: "Text is required");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact(Skip = "no longer used, will refactor out")]
        public async Task ShouldThrowValidationExceptionsOnProcessIfPostCodesDontMatchAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string postCode = GetRandomString();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.ExtractPostCode(randomResolvedAddress.PostalAddress))
                    .Returns(postCode);

            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message: "Invalid address persistence orchestration argument, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressPersistanceOrchestrationException.AddData(
                key: "postCode",
                values: "PostCodes need to match.");

            var expectedAddressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: invalidArgumentAddressPersistanceOrchestrationException);

            // when
            ValueTask<ResolvedAddress> processResolvedAddressesTask =
                this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(
                    resolvedAddresses: randomResolvedAddress);

            AddressPersistenceOrchestrationValidationException
                actualAddressPersistanceOrchestrationValidationException =
                    await Assert.ThrowsAsync<AddressPersistenceOrchestrationValidationException>(
                        processResolvedAddressesTask.AsTask);

            //then
            actualAddressPersistanceOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressPersistanceOrchestrationValidationException);

            this.addressMatcherProcessingServiceMock.Verify(processing =>
                processing.ExtractPostCode(randomResolvedAddress.PostalAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}