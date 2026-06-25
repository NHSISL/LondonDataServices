// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Clients.AddressToUprnClients
{
    public partial class AddressToUprnClientTests
    {
        [Fact]
        public async Task ShouldMatchAddressToUprnAsync()
        {
            // given
            Stream randomData = CreateRandomStream();
            string randomFileName = GetRandomString();
            Guid randomCorrelationId = GetRandomGuid();
            CancellationToken cancellationToken = CancellationToken.None;

            this.addressToUprnOrchestrationServiceMock.Setup(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken))
                .Returns(ValueTask.CompletedTask);

            // when
            await this.addressToUprnClient.MatchAddressToUprnAsync(
                data: randomData,
                filename: randomFileName,
                correlationId: randomCorrelationId,
                cancellationToken: cancellationToken);

            // then
            this.addressToUprnOrchestrationServiceMock.Verify(orchestration =>
                orchestration.MatchAddressToUprnAsync(
                    randomData,
                    randomFileName,
                    randomCorrelationId,
                    cancellationToken),
                        Times.Once);

            this.addressToUprnOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
