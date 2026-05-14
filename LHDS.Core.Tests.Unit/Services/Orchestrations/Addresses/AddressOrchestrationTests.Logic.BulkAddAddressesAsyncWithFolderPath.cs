// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Services.Orchestrations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldImportOrdinanceAddressesAsyncWithFolderPath()
        {
            // Given
            var addressOrchestrationServiceMock = new Mock<AddressOrchestrationService>
               (this.addressProcessingServiceMock.Object,
                this.assignProcessingServiceMock.Object,
                this.fileBrokerMock.Object,
                this.csvHelperBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.auditBrokerMock.Object,
                this.loggingBrokerMock.Object,
                this.identifierBrokerMock.Object)
            { CallBase = true };

            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFolderPath = Path.Combine(
                Path.GetDirectoryName(assembly),
                "Resources/Services/Orchestrations/Addresses/");

            addressOrchestrationServiceMock.Setup(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(
                    inputFolderPath))
                    .Returns(ValueTask.CompletedTask);

            AddressOrchestrationService service = addressOrchestrationServiceMock.Object;

            // When
            await service.BulkAddAddressesAsync(inputFolderPath);

            // Then
            addressOrchestrationServiceMock.Verify(service =>
                service.ReadCsvDataAndBulkAddAddressesAsync(
                    inputFolderPath),
                    Times.Once);

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

