// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessDataIfAddressIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidAddress = invalidText;

            var invalidArgumentAddressNormalisationOrchestrationException =
                new InvalidArgumentAddressNormalisationOrchestrationException(
                    message:
                    "Invalid address normalisation orchestration argument. Please correct the errors and try again.");

            invalidArgumentAddressNormalisationOrchestrationException.AddData(
                key: "address",
                values: "Text is required");

            var expectedAddressNormalisationOrchestrationValidationException =
                new AddressNormalisationOrchestrationValidationException(
                    message: "Address normalisation orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentAddressNormalisationOrchestrationException);

            // when
            ValueTask<List<AddressNormalisation>> addAddressNormalisationTask =
                this.addressNormalisationOrchestrationService.ProcessDataAsync(invalidAddress);

            AddressNormalisationOrchestrationValidationException actualAddressNormalisationOrchestrationValidationException =
                await Assert.ThrowsAsync<AddressNormalisationOrchestrationValidationException>(() =>
                    addAddressNormalisationTask.AsTask());

            // then
            actualAddressNormalisationOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedAddressNormalisationOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressNormalisationOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressLoadingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}