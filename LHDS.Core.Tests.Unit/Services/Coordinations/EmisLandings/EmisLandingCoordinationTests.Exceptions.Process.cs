// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowAggregateExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependancyValidationException)
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
                        .ThrowsAsync(dependancyValidationException);

                var emisLandingCoordinationDependencyValidationException =
                    new EmisLandingCoordinationDependencyValidationException(
                        message: "EMIS landing coordination dependency validation error occurred, please try again.",
                        innerException: dependancyValidationException.InnerException as Xeption);

                exceptions.Add(emisLandingCoordinationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to process files for {exceptions.Count} subscriber agreements",
                    exceptions);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service occurred, please contact support.",
                    innerException: aggregateException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService.ProcessAsync();

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(async () =>
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

            var emisLandingCoordinationDependencyValidationLoggingException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     emisLandingCoordinationDependencyValidationLoggingException))),
                         Times.Exactly(randomActiveSubscriberAgreementIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

