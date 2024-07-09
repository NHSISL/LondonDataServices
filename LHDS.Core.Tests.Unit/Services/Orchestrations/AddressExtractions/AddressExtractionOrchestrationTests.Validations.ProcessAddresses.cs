// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessAddressIfDataIsNullAndLogItAsync(string invalidText)
        {
            // given
            byte[] invalidData = null;
            string invalidFileName = invalidText;

            var invalidArgumentAddressExtractionOrchestrationException =
                new InvalidArgumentAddressExtractionOrchestrationException(
                    message: "Invalid argument address extraction orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentAddressExtractionOrchestrationException.AddData(
                key: "data",
                values: "Data is required");

            invalidArgumentAddressExtractionOrchestrationException.AddData(
                key: "filename",
                values: "Text is required");

            var expectedAddressExtractionValidationOrchestrationException =
                new AddressExtractionValidationOrchestrationException(
                    message: "Address extraction orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentAddressExtractionOrchestrationException);

            // when
            ValueTask<List<Address>> processDataTask =
                this.addressExtractionOrchestrationService.BulkAddAddressesAsync(invalidData, invalidFileName);

            AddressExtractionValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<AddressExtractionValidationOrchestrationException>(
                    processDataTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedAddressExtractionValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressExtractionValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
