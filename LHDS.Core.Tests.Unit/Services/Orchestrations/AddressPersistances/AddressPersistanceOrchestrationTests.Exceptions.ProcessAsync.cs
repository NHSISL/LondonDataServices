// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressPersistenceDependencyValidationExceptions))]
        public async Task ShouldThrowAggregateDependencyValidationOnAddressPersistenceIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses(GetRandomNumber()).ToList();
            string someFileName = GetRandomString();
            List<Exception> exceptions = new List<Exception>();

            foreach (Address address in randomAddresses)
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
        [MemberData(nameof(AddressPersistenceDependencyExceptions))]
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddressPersistIfServiceErrorOccursAsync()
        {
            // given
            var mock = new Mock<AddressPersistanceOrchestrationService>(
                addressProcessingServiceMock.Object,
                addressMatcherProcessingServiceMock.Object,
                resolvedAddressProcessingServiceMock.Object,
                auditBrokerMock.Object,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object)
            { CallBase = true };

            List<Address> randomAddresses = CreateRandomAddresses(1).ToList();
            Address address = randomAddresses[0];
            string exceptionMessage = GetRandomString();
            string someFileName = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            mock.Setup(x => x.ValidateAddressPersistenceOrchestration(randomAddresses, someFileName))
                .Throws(serviceException);

            AddressPersistanceOrchestrationService addressPersistanceOrchestrationService = mock.Object;

            var failedAddressPersistanceOrchestrationServiceException =
                new FailedAddressPersistenceOrchestrationServiceException(
                    message: "Failed address persistence orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, please contact support.",
                    innerException: failedAddressPersistanceOrchestrationServiceException);

            // when
            ValueTask<List<Address>> processTask =
                addressPersistanceOrchestrationService.PersistAddressAsync(randomAddresses, someFileName);

            AddressPersistenceOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<AddressPersistenceOrchestrationServiceException>(
                    processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedAddressPersistanceOrchestrationServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressPersistanceOrchestrationServiceException))),
                       Times.Once);

            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}