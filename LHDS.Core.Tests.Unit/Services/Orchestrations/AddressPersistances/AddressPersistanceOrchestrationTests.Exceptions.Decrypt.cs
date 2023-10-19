// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(AddressPersistanceDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnAddressPersistanceIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            Address firstAddress = randomAddresses[0];

            var expectedDependencyException =
                new AddressPersistanceOrchestrationDependencyValidationException(
                    message:
                    "Address persistance orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressProcessingServiceMock.Setup(service =>
               service.ModifyOrAddAddressAsync(firstAddress)
                   .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Address>> processTask = 
                this.addressPersistanceOrchestrationService.ProcessAsync(randomAddresses);

            AddressPersistanceOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<AddressPersistanceOrchestrationDependencyValidationException>(
                    processTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.addressNormalisationProcessingServiceMock.Verify(service =>
             service.GetNormalisedAddress(It.IsAny<string>()),
                 Times.Once);

            this.addressProcessingServiceMock.Verify(service =>
             service.ModifyOrAddAddressAsync(It.IsAny<Address>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}