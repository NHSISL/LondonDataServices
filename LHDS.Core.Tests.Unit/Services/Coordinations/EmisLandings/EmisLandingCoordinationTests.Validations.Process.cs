// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessIfSubscriberCredentialIsNullAndLogItAsync()
        {
            // Given
            int randomNumber = GetRandomNumber();
            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIds())
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId))
                        .ReturnsAsync((SubscriberCredential)null);

                var nullSupplierCredentialEmisLandingCoordinationException =
                    new NullSupplierCredentialEmisLandingCoordinationException(
                        message: $"Subscriber credentials not found for Id: {subscriberAgreementId}, " +
                            $"please correct the errors and try again.");

                var emisLandingCoordinationValidationException =
                    new EmisLandingCoordinationValidationException(
                        message: "Address coordination validation error occurred, please try again.",
                        innerException: nullSupplierCredentialEmisLandingCoordinationException);

                exceptions.Add(emisLandingCoordinationValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to process files for {exceptions.Count} subsciber agreements",
                    exceptions);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service occurred, please contact support",
                    innerException: aggregateException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS coordination service error occurred, contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService.ProcessAsync();

            EmisLandingCoordinationValidationException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationValidationException>(async () =>
                    await processDataTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIds(),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId),
                        Times.Once);
            }

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

