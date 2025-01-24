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
        public async Task ShouldThrowAggregateDependencyValidationExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            int randomNumber = GetRandomNumber();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()))
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
                    message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService.ProcessAsync(
                supplierId: inputSupplierId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()),
                        Times.Once);
            }

            var emisLandingCoordinationDependencyValidationLoggingException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     emisLandingCoordinationDependencyValidationLoggingException))),
                         Times.Exactly(randomActiveSubscriberAgreementIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowAggregateDependencyExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            int randomNumber = GetRandomNumber();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()))
                        .ThrowsAsync(dependancyValidationException);

                var emisLandingCoordinationDependencyException =
                    new EmisLandingCoordinationDependencyException(
                        message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                        innerException: dependancyValidationException.InnerException as Xeption);

                exceptions.Add(emisLandingCoordinationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to process files for {exceptions.Count} subscriber agreements",
                    exceptions);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()),
                        Times.Once);
            }

            var emisLandingCoordinationDependencyLoggingException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     emisLandingCoordinationDependencyLoggingException))),
                         Times.Exactly(randomActiveSubscriberAgreementIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnProcessIfErrorsInLoopAndLogItAsync()
        {
            // Given
            int randomNumber = 1; // GetRandomNumber();
            var serviceException = new Exception();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ReturnsAsync(randomActiveSubscriberAgreementIds);

            var innerFailedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var innerEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: innerFailedEmisLandingCoordinationServiceException);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Setup(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerEmisLandingCoordinationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to process files for {exceptions.Count} subscriber agreements",
                    exceptions);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                    innerException: aggregateException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            foreach (Guid subscriberAgreementId in randomActiveSubscriberAgreementIds)
            {
                this.subscriberCredentialOrchestrationMock.Verify(service =>
                    service.RetrieveSubscriberCredentialByIdAsync(subscriberAgreementId, It.IsAny<bool>()),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     innerEmisLandingCoordinationServiceException))),
                         Times.Exactly(randomActiveSubscriberAgreementIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            int randomNumber = GetRandomNumber();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ThrowsAsync(dependancyValidationException);

            var expectedEmisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            EmisLandingCoordinationDependencyValidationException
                actualEmisLandingCoordinationDependencyValidationException =
                    await Assert.ThrowsAsync<EmisLandingCoordinationDependencyValidationException>(
                        processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyValidationException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyValidationException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnProcessIfErrorsInLoopAndLogItAsync(
            Xeption dependancyValidationException)
        {
            // Given
            int randomNumber = GetRandomNumber();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ThrowsAsync(dependancyValidationException);

            var expectedEmisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            EmisLandingCoordinationDependencyException actualEmisLandingCoordinationDependencyException =
                await Assert.ThrowsAsync<EmisLandingCoordinationDependencyException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationDependencyException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationDependencyException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationDependencyException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessIfErrorsInLoopAndLogItAsync()
        {
            // Given
            int randomNumber = GetRandomNumber();
            var serviceException = new Exception();
            Guid inputSupplierId = Guid.NewGuid();

            List<Guid> randomActiveSubscriberAgreementIds =
                CreateRandomActiveSubscriberAgreementIds(number: randomNumber);

            List<string> randomEmisLandingPaths = CreateRandomLandingPaths(number: GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.subscriberCredentialOrchestrationMock.Setup(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync())
                    .ThrowsAsync(serviceException);

            var failedEmisLandingCoordinationServiceException =
                new FailedEmisLandingCoordinationServiceException(
                    message: "Failed EMIS landing coordination service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedEmisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: failedEmisLandingCoordinationServiceException);

            // When
            ValueTask<List<string>> processDataTask = this.emisLandingCoordinationService
                .ProcessAsync(supplierId: inputSupplierId);

            EmisLandingCoordinationServiceException actualEmisLandingCoordinationValidationException =
                await Assert.ThrowsAsync<EmisLandingCoordinationServiceException>(
                    processDataTask.AsTask);

            // Then
            actualEmisLandingCoordinationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingCoordinationServiceException);

            this.subscriberCredentialOrchestrationMock.Verify(service =>
                service.RetrieveAllActiveSubscriberCredentialIdsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(IsSameExceptionAs(
                     expectedEmisLandingCoordinationServiceException))),
                         Times.Once);

            this.subscriberCredentialOrchestrationMock.VerifyNoOtherCalls();
            this.emisLandingOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

