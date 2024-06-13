// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyValidationExceptions))]
        public async Task ShouldThrowAggregateDependencyValidationOnAddressPersistenceIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<SubscriberAgreement> randomSubscriberAgreements = CreateRandomSubscriberAgreements();
            List<Exception> exceptions = new List<Exception>();

            foreach ( address in randomAddresses)
            {
                this.addressProcessingServiceMock.Setup(processing =>
                    processing.ModifyOrAddAddressAsync(It.IsAny<Address>()))
                        .ThrowsAsync(dependencyValidationException);

                var addressPersistanceOrchestrationDependencyValidationException =
                    new AddressPersistenceOrchestrationDependencyValidationException(
                        message: "Address persistence orchestration dependency validation error occurred, " +
                            "please try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(addressPersistanceOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify {exceptions.Count} address(es)",
                    exceptions);

            var failedAddressPersistanceOrchestrationServiceException =
                 new FailedAddressPersistenceOrchestrationServiceException(
                     message: "Failed address persistence aggregate orchestration service error occurred, " +
                     "please contact support.",
                     innerException: aggregateException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses, someFileName);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.addressProcessingServiceMock.Verify(service =>
                service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                    Times.Exactly(randomAddresses.Count));

            var addressPersistanceDependencyValidationException =
                new AddressPersistenceOrchestrationDependencyValidationException(
                    message: "Address persistence orchestration dependency validation error occurred, " +
                        "please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressPersistanceDependencyValidationException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationServiceException))),
                        Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyExceptions))]
        public async Task ShouldThrowAggregateDependencyOnAddressPersistenceIfDependencyOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(GetRandomNumber()).ToList();
            string someFileName = GetRandomString();
            List<Exception> exceptions = new List<Exception>();

            foreach (Address address in randomAddresses)
            {
                this.addressProcessingServiceMock.Setup(processing =>
                    processing.ModifyOrAddAddressAsync(It.IsAny<Address>()))
                        .ThrowsAsync(dependencyException);

                var addressPersistanceOrchestrationDependencyException =
                    new AddressPersistenceOrchestrationDependencyException(
                        message: "Address persistence orchestration dependency error occurred, " +
                            "please try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(addressPersistanceOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify {exceptions.Count} address(es)",
                    exceptions);

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence aggregate orchestration service error occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses, someFileName);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                  Times.Exactly(randomAddresses.Count));

            var addressPersistanceDependencyException =
                new AddressPersistenceOrchestrationDependencyException(
                    message: "Address persistence orchestration dependency error occurred, " +
                        "please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    addressPersistanceDependencyException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressPersistanceOrchestrationServiceException))),
                        Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnAddressPersistenceIfErrorsAndLogItAsync()
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(GetRandomNumber()).ToList();
            string someFileName = GetRandomString();
            var serviceException = new Exception();
            List<Exception> exceptions = new List<Exception>();

            var innerfailedAddressPersistenceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence orchestration service error occurred, " +
                        "please contact support.",
                    innerException: serviceException);

            var innerAddressPersistenceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: innerfailedAddressPersistenceOrchestrationServiceException);

            foreach (Address address in randomAddresses)
            {
                this.addressProcessingServiceMock.Setup(processing =>
                    processing.ModifyOrAddAddressAsync(It.IsAny<Address>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerAddressPersistenceOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to add or modify {exceptions.Count} address(es)",
                        exceptions);

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedAddressPersistenceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                this.addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses, someFileName);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(async () =>
                    await processTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedAddressPersistenceOrchestrationServiceException);

            foreach (Address address in randomAddresses)
            {
                this.addressProcessingServiceMock.Verify(service =>
                   service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                       Times.Exactly(randomAddresses.Count));
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    innerAddressPersistenceOrchestrationServiceException))),
                        Times.Exactly(randomAddresses.Count));

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressPersistenceOrchestrationServiceException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveAllSubscriberCredentialIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);

            var expectedDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyValidationException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialOrchestrationDependencyValidationException actualDependencyValidationException =
                Assert.Throws<SubscriberCredentialOrchestrationDependencyValidationException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualDependencyValidationException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyValidationException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnretrieveAllSubscriberCredentialIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);

            var expectedDependencyException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialDependencyOrchestrationException actualDependencyVException =
                Assert.Throws<SubscriberCredentialDependencyOrchestrationException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualDependencyVException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnRetrieveAllSubscriberCredentialIfServiceErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
            var serviceException = new Exception();

            var failedSubscriberCredentialOrchestrationServiceException =
                new FailedSubscriberCredentialOrchestrationServiceException(
                    message: "Failed subscriber credential orchestration service error occurred, " +
                    "please contact support.",
                    innerException: serviceException);

            var expectedSerivceException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, " +
                        "fix the errors and try again.",
                    innerException: failedSubscriberCredentialOrchestrationServiceException);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
               service.RetrieveAllSubscriberAgreements())
                   .Throws(serviceException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialOrchestrationServiceException actualServiceException =
                Assert.Throws<SubscriberCredentialOrchestrationServiceException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualServiceException.Should()
                 .BeEquivalentTo(expectedSerivceException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSerivceException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}