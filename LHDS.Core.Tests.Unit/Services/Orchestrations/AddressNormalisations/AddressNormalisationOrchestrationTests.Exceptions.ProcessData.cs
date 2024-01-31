// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
        ShouldThrowDependencyValidationExceptionOnGetNormalisedAddressIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            var expectedAddressNormalisationOrchestrationDependencyValidationException =
                new AddressNormalisationOrchestrationDependencyValidationException(
                    message: "Address normalisation orchestration dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.addressParserProcessingServiceMock.Setup(processing =>
                processing.ProcessCsvAsync(inputAddress))
                    .Throws(dependencyValidationException);

            // when
            List<AddressNormalisation> actualAddressesTask =
                await this.addressNormalisationOrchestrationService.ProcessDataAsync(inputAddress);

            AddressNormalisationOrchestrationDependencyValidationException actualException =
                Assert.Throws<AddressNormalisationOrchestrationDependencyValidationException>(
                    () => this.addressNormalisationOrchestrationService.ProcessDataAsync(inputAddress).Result);

            // then
            actualException.Should().BeEquivalentTo(
                expectedAddressNormalisationOrchestrationDependencyValidationException);

            this.addressParserProcessingServiceMock.Verify(processing =>
                processing.ProcessCsvAsync(inputAddress),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAddressNormalisationOrchestrationDependencyValidationException))),
                         Times.Once);

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
