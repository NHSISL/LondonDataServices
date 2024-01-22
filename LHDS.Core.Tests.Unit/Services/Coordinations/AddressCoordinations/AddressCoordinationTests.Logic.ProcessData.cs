// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessDataAndLogAsync()
        {
            // Given
            byte[] inputData = Encoding.UTF8.GetBytes(GetRandomString());
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> extractedAddresses = randomAddresses.DeepClone();
            List<Address> persistedAddresses = extractedAddresses.DeepClone();

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessDataAsync(inputData))
                    .ReturnsAsync(extractedAddresses);

            this.addressPersistanceOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(extractedAddresses))
                    .ReturnsAsync(persistedAddresses);

            List<Address> expectedAddresses = persistedAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressCoordinationService.ProcessData(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
                service.ProcessDataAsync(inputData),
                    Times.Once());

            this.addressPersistanceOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(extractedAddresses),
                    Times.Once());

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

