// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldLoadAddressDataAndLogAsyncWithPath()
        {
            // Given
            string someFolderPath = GetRandomString();

            // When
            await this.addressCoordinationService.LoadAddressDataAsync(someFolderPath);

            // Then

            this.addressOrchestrationServiceMock.Verify(service =>
                service.BulkAddAddressesAsync(someFolderPath),
                    Times.Once);

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

