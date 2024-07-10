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
        public async Task ShouldMatchAddressDataFromFileAsync()
        {
            // Given
            string someFilename = GetRandomString();
            byte[] inputData = Encoding.UTF8.GetBytes(GetRandomString());
            Stream inputStream = new MemoryStream(inputData);

            // When
            await this.addressCoordinationService
                .LoadAddressesToResolveAsync(inputStream, someFilename);

            // Then
            this.resolvedAddressOrchestrationServiceMock.Verify(service =>
                service.UploadAddressesToReslveAsync(inputStream, someFilename),
                    Times.Once());

            this.addressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}