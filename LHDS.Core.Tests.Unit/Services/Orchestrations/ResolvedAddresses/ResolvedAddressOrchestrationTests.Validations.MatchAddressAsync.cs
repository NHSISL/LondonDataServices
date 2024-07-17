// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchIfAssignUPRNIsNullAndLogItAsync()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: GetRandomNumber());
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;

            string inputResolvedAddress = unmatchedResolvedAddresses
                .FirstOrDefault().UnstructuredPostalAddress;

            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddress = randomAssignAddress;
            storageAssignAddress.UPRN = 0;

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddresses())
                   .Returns(unmatchedResolvedAddresses.AsQueryable())
                   .Returns(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            ResolvedAddress processingResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().DeepClone();
            ResolvedAddress lockedResolvedAddress = processingResolvedAddress;
            lockedResolvedAddress.IsProcessing = true;
            lockedResolvedAddress.RetryCount += 1;
            lockedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            ResolvedAddress finalUpdate = lockedResolvedAddress.DeepClone();

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(lockedResolvedAddress))
                    .ReturnsAsync(lockedResolvedAddress);

            this.assignProcessingServiceMock.Setup(processing =>
                processing.MatchAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageAssignAddress);

            var nullUPRNResolvedAddressOrchestrationException =
                new NullUPRNResolvedAddressOrchestrationException(
                    message: "Null UPRN Resolved Address orchestration exception, " +
                        "please correct the errors and try again.");

            nullUPRNResolvedAddressOrchestrationException.AddData(
                key: "UPRN",
                values: "UPRN is required");

            var expectedResolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: nullUPRNResolvedAddressOrchestrationException);

            // when
            ValueTask matchAddressesTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationValidationException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationValidationException>(
                    matchAddressesTask.AsTask);

            // then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationValidationException);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
              service.RetrieveAllResolvedAddresses(),
                  Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
              broker.GetCurrentDateTimeOffset(),
                  Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))),
                    Times.Once());

            this.assignProcessingServiceMock.Verify(processing =>
                processing.MatchAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressOrchestrationValidationException))),
                       Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
