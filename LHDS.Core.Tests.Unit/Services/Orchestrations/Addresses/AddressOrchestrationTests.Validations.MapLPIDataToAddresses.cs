// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnMapLPIDataIfFileDoesNotExistAsync()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string invalidFileName = GetRandomString();

            var invalidFileAddressOrchestrationException =
                new InvalidFileAddressOrchestrationException(
                    message: $"The file {invalidFileName} could not be found.");

            InvalidFileAddressOrchestrationException expectedFileAddressOrchestrationException =
                invalidFileAddressOrchestrationException;

            this.fileBrokerMock.Setup(broker =>
                broker.CheckIfFileExistsAsync(invalidFileName))
                    .ReturnsAsync(false);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            ValueTask<List<Address>> mapLpiDataTask = service.MapLPIDataToAddressesAsync(invalidFileName);

            InvalidFileAddressOrchestrationException actualFileAddressOrchestrationException =
                await Assert.ThrowsAsync<InvalidFileAddressOrchestrationException>(
                    mapLpiDataTask.AsTask);

            // Then
            actualFileAddressOrchestrationException.Should()
                .BeEquivalentTo(expectedFileAddressOrchestrationException);

            this.fileBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.assignProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

