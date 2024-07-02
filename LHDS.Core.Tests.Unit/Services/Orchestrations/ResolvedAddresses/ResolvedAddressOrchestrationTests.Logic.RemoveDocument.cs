// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        [Fact]
        public async Task ShouldRemoveDocumentAsync()
        {
            // Given
            var randomContainer = GetRandomString();
            var randomFileName = GetRandomString();
            var randomfileData = Encoding.UTF8.GetBytes(GetRandomString());

            // When
            await this.resolvedAddressOrchestrationService
                .RemoveDocumentByFileNameAsync(fileName: randomFileName, container: randomContainer);

            // Then
            this.documentProcessingServiceMock.Verify(service =>
                service.RemoveDocumentByFileNameAsync(randomFileName, randomContainer),
                    Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.resolvedAddressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
