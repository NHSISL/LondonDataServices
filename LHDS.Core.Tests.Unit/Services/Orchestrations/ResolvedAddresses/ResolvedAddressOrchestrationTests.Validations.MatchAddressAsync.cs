// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMatchIfMappedResolvedAddressIsNullAndLogItAsync()
        {
            var resolvedAddressOrchestrationServiceMock = new Mock<ResolvedAddressOrchestrationService>(
                this.documentProcessingServiceMock.Object,
                this.resolvedAddressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.addressProcessingServiceMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.identifierBrokerMock.Object,
                this.securityBrokerMock.Object,

                new BlobContainers
                {
                    Addresses = "addresses"
                })
                { CallBase = true };

            Guid identifier = Guid.NewGuid();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            List<ResolvedAddress> unmatchedResolvedAddresses = randomResolvedAddresses;
            List<Exception> exceptions = new List<Exception>();

            string inputResolvedAddress = unmatchedResolvedAddresses
                .FirstOrDefault().UnstructuredPostalAddress;

            AssignAddress randomAssignAddress = CreateRandomAssignAddress(randomDateTimeOffset);
            AssignAddress storageAssignAddress = randomAssignAddress;
            storageAssignAddress.BestMatch.UPRN = "";

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.securityBrokerMock.Setup(broker =>
               broker.GetCurrentUserAsync())
                   .ReturnsAsync(randomEntraUser);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(unmatchedResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            ResolvedAddress processingResolvedAddress = unmatchedResolvedAddresses.FirstOrDefault().DeepClone();
            ResolvedAddress lockedResolvedAddress = processingResolvedAddress.DeepClone();
            lockedResolvedAddress.IsProcessing = true;
            lockedResolvedAddress.RetryCount += 1;
            lockedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            ResolvedAddress updatedResolvedAddress = lockedResolvedAddress.DeepClone();
            ResolvedAddress mappedResolvedAddress = updatedResolvedAddress.DeepClone();
            mappedResolvedAddress.UPRN = null;
            mappedResolvedAddress.UpdatedDate = randomDateTimeOffset;
            mappedResolvedAddress.IsProcessed = true;
            ResolvedAddress failedToProcessResolvedAddress = updatedResolvedAddress.DeepClone();
            failedToProcessResolvedAddress.IsProcessing = false;
            failedToProcessResolvedAddress.UpdatedDate = randomDateTimeOffset;
            ResolvedAddress? nullResolvedAddress = null;

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))))
                    .ReturnsAsync(updatedResolvedAddress);

            this.resolvedAddressProcessingServiceMock.Setup(processing =>
                processing.RetrieveResolvedAddressByIdAsync(updatedResolvedAddress.Id))
                    .ReturnsAsync(failedToProcessResolvedAddress);

            this.assignProcessingServiceMock.Setup(processing =>
                processing.MatchAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageAssignAddress);

            resolvedAddressOrchestrationServiceMock.Setup(service =>
                service.MapOrdananceWithAssign(
                    It.Is(SameResolvedAddressAs(updatedResolvedAddress)),
                    It.Is(SameAssignAddressAs(storageAssignAddress)),
                    null))
                        .Returns(nullResolvedAddress);

            var nullResolvedAddressOrchestrationException =
                new NullResolvedAddressOrchestrationException(
                    message: "Null Resolved Address orchestration exception, " +
                        "please correct the errors and try again.");

            var resolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: nullResolvedAddressOrchestrationException);

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                exceptions.Add(resolvedAddressOrchestrationValidationException);
            }

            var aggregateException =
               new AggregateException(
                   $"Unable to retrieve {exceptions.Count} resolved addresses.",
                   exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred" +
                        ", please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message:
                        "Resolved address orchestration service error occurred, please contact support.",
                    failedResolvedAddressOrchestrationServiceException);

            ResolvedAddressOrchestrationService service = resolvedAddressOrchestrationServiceMock.Object;

            // when
            ValueTask matchAddressesTask = service.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationValidationException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(
                    matchAddressesTask.AsTask);

            // then
            actualResolvedAddressOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(service =>
              service.RetrieveAllResolvedAddressesAsync(),
                  Times.Exactly(2));

            this.dateTimeBrokerMock.Verify(broker =>
              broker.GetCurrentDateTimeOffsetAsync(),
                  Times.Exactly(3));

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(lockedResolvedAddress))),
                    Times.Once());

            this.assignProcessingServiceMock.Verify(processing =>
                processing.MatchAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.RetrieveResolvedAddressByIdAsync(updatedResolvedAddress.Id),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
                processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(failedToProcessResolvedAddress))),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    resolvedAddressOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedResolvedAddressOrchestrationServiceException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}
