// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnMatchIfDependencyValidationErrorOccursAndLogItAsync(
                Xeption dependencyValidationException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyValidationException>(
                    matchAddressDataTask.AsTask);

            // then
            actualException.Should().
                BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyValidationException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyValidationException))),
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

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMatchIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedResolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationDependencyException>(
                    matchAddressDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationDependencyException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationDependencyException))),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMatchIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedResolvedAddressOrchestrationServiveException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock
                .Setup(broker => broker.GetIdentifierAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(matchAddressDataTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedResolvedAddressOrchestrationServiveException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedResolvedAddressOrchestrationServiveException))),
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

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnMatchErrorsInLoopAndLogItAsync(
                Xeption dependencyValidationException)
        {
            // Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            List<Exception> exceptions = new List<Exception>();

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(randomResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingUnMatchedResolvedAddress.IsProcessing = true;
                processesingUnMatchedResolvedAddress.RetryCount += 1;
                processesingUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                 processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(processesingUnMatchedResolvedAddress))))
                     .ThrowsAsync(dependencyValidationException);

                ResolvedAddress UnMatchedResolvedAddressById = processesingUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(processesingUnMatchedResolvedAddress.Id))
                            .ReturnsAsync(UnMatchedResolvedAddressById);

                var innerResolvedAddressOrchestrationDependencyValidationException =
                    new ResolvedAddressOrchestrationDependencyValidationException(
                        message: "Resolved address orchestration dependency validation errors occurred, " +
                            "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(innerResolvedAddressOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(matchAddressDataTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly((randomResolvedAddresses.Count) + 1));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.RetrieveAllResolvedAddressesAsync(),
                   Times.Exactly(randomResolvedAddresses.Count + 1));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(1 + (randomResolvedAddresses.Count * 2)));

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingVUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingVUnMatchedResolvedAddress.IsProcessing = true;
                processesingVUnMatchedResolvedAddress.IsProcessed = false;
                processesingVUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(processesingVUnMatchedResolvedAddress))),
                            Times.Once);

                ResolvedAddress cleanedUnMatchedResolvedAddress = processesingVUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(cleanedUnMatchedResolvedAddress.Id),
                            Times.Once);

                cleanedUnMatchedResolvedAddress.IsProcessing = false;
                cleanedUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(cleanedUnMatchedResolvedAddress))),
                            Times.Once);
            }

            var resolvedAddressOrchestrationDependencyValidationLoggingException =
               new ResolvedAddressOrchestrationDependencyValidationException(
                   message: "Resolved address orchestration dependency validation errors occurred, " +
                       "please try again.",
                   innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    resolvedAddressOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
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

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnMatchErrorsInLoopAndLogItAsync(
                Xeption dependencyException)
        {
            // Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            List<Exception> exceptions = new List<Exception>();

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(randomResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingUnMatchedResolvedAddress.IsProcessing = true;
                processesingUnMatchedResolvedAddress.RetryCount += 1;
                processesingUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                 processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(processesingUnMatchedResolvedAddress))))
                     .ThrowsAsync(dependencyException);

                ResolvedAddress UnMatchedResolvedAddressById = processesingUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(processesingUnMatchedResolvedAddress.Id))
                            .ReturnsAsync(UnMatchedResolvedAddressById);

                var innerResolvedAddressOrchestrationDependencyException =
                    new ResolvedAddressOrchestrationDependencyException(
                        message: "Resolved address orchestration dependency errors occurred, please contact support.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(innerResolvedAddressOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
               await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(matchAddressDataTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly((randomResolvedAddresses.Count) + 1));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.RetrieveAllResolvedAddressesAsync(),
                   Times.Exactly(randomResolvedAddresses.Count + 1));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(1 + (randomResolvedAddresses.Count * 2)));

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingVUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingVUnMatchedResolvedAddress.IsProcessing = true;
                processesingVUnMatchedResolvedAddress.IsProcessed = false;
                processesingVUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(processesingVUnMatchedResolvedAddress))),
                            Times.Once);

                ResolvedAddress cleanedUnMatchedResolvedAddress = processesingVUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(cleanedUnMatchedResolvedAddress.Id),
                            Times.Once);

                cleanedUnMatchedResolvedAddress.IsProcessing = false;
                cleanedUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(cleanedUnMatchedResolvedAddress))),
                            Times.Once);
            }

            var resolvedAddressOrchestrationDependencyLoggingException =
               new ResolvedAddressOrchestrationDependencyException(
                   message: "Resolved address orchestration dependency errors occurred, please contact support.",
                   innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    resolvedAddressOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
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

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnMatchErrorsInLoopAndLogItAsync()
        {
            // Given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomUnmatchedAddresses(count: 1);
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

            this.resolvedAddressProcessingServiceMock.SetupSequence(service =>
               service.RetrieveAllResolvedAddressesAsync())
                   .ReturnsAsync(randomResolvedAddresses.AsQueryable())
                   .ReturnsAsync(new List<ResolvedAddress>().AsQueryable());

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            var innerFailedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: innerFailedResolvedAddressOrchestrationServiceException);

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingUnMatchedResolvedAddress.IsProcessing = true;
                processesingUnMatchedResolvedAddress.RetryCount += 1;
                processesingUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(
                        processesingUnMatchedResolvedAddress))))
                            .ThrowsAsync(serviceException);

                ResolvedAddress UnMatchedResolvedAddressById = processesingUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Setup(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(processesingUnMatchedResolvedAddress.Id))
                        .ReturnsAsync(UnMatchedResolvedAddressById);

                exceptions.Add(innerResolvedAddressOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} ResolvedAddresses",
                    exceptions);

            var failedResolvedAddressOrchestrationServiceException =
                new FailedResolvedAddressOrchestrationServiceException(
                    message: "Failed resolved address aggregate orchestration service errors occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedResolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    innerException: failedResolvedAddressOrchestrationServiceException);

            // When
            ValueTask matchAddressDataTask = this.resolvedAddressOrchestrationService.MatchAddressDataAsync();

            ResolvedAddressOrchestrationServiceException actualResolvedAddressOrchestrationServiceException =
                await Assert.ThrowsAsync<ResolvedAddressOrchestrationServiceException>(matchAddressDataTask.AsTask);

            // Then
            actualResolvedAddressOrchestrationServiceException.Should()
               .BeEquivalentTo(expectedResolvedAddressOrchestrationServiceException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly((randomResolvedAddresses.Count) + 1));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Once());

            this.resolvedAddressProcessingServiceMock.Verify(processing =>
               processing.RetrieveAllResolvedAddressesAsync(),
                   Times.Exactly(randomResolvedAddresses.Count + 1));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(1 + (randomResolvedAddresses.Count * 2)));

            foreach (ResolvedAddress unMatchedResolvedAddress in randomResolvedAddresses)
            {
                ResolvedAddress processesingUnMatchedResolvedAddress = unMatchedResolvedAddress.DeepClone();
                processesingUnMatchedResolvedAddress.IsProcessing = true;
                processesingUnMatchedResolvedAddress.IsProcessed = false;
                processesingUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(
                        processesingUnMatchedResolvedAddress))),
                            Times.Once);

                ResolvedAddress cleanedUnMatchedResolvedAddress = processesingUnMatchedResolvedAddress.DeepClone();

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.RetrieveResolvedAddressByIdAsync(cleanedUnMatchedResolvedAddress.Id),
                        Times.Once);

                cleanedUnMatchedResolvedAddress.IsProcessing = false;
                cleanedUnMatchedResolvedAddress.UpdatedDate = randomDateTimeOffset;

                this.resolvedAddressProcessingServiceMock.Verify(processing =>
                    processing.ModifyResolvedAddressAsync(It.Is(SameResolvedAddressAs(
                        cleanedUnMatchedResolvedAddress))),
                            Times.Once);
            };

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerResolvedAddressOrchestrationServiceException))),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualResolvedAddressOrchestrationServiceException))),
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
