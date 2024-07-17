// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldUploadResolvedAddressAsync()
        {
            // Given
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            string inputContent = GetRandomString();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputContent);
            Stream inputStream = new MemoryStream(inputBytes);
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            List<ResolvedAddress> mappedResolvedAddresses = randomResolvedAddresses;

            Dictionary<string, int> fieldMappings =
                new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                };

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings))
                    .ReturnsAsync(mappedResolvedAddresses);

            this.resolvedAddressProcessingServiceMock.Setup(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()))
                    .Returns(ValueTask.CompletedTask);

            // When
            await this.resolvedAddressOrchestrationService
                .UploadAddressesToResolveAsync(input: inputStream, fileName: inputFileName);

            // Then

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(inputContent, true, fieldMappings),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.Verify(service =>
                service.BulkAddResolvedAddressesAsync(mappedResolvedAddresses, It.IsAny<string>()),
                    Times.Once);

            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
