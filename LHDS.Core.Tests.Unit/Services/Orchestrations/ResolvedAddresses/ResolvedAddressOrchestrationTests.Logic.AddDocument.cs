// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldAddFileAsync()
        {
            // Given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomfileData = Encoding.ASCII.GetBytes(GetRandomString());

            // When
            await this.resolvedAddressOrchestrationService
                .AddDocumentAsync(data: randomfileData, fileName: randomFileName, container: randomContainer);

            // Then
            //this.documentProcessingServiceMock.Verify(service =>
            //    service.AddDocumentAsync(It.Is<Document>(doc =>
            //        doc.FileName == randomFileName && doc.DocumentData == randomfileData), randomContainer),
            //        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
