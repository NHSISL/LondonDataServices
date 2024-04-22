// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePdsAuditByIdAsync()
        {
            // given
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit storagePdsAudit = randomPdsAudit;
            PdsAudit expectedPdsAudit = storagePdsAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAudit.Id))
                    .ReturnsAsync(storagePdsAudit);

            // when
            PdsAudit actualPdsAudit =
                await this.pdsAuditService.RetrievePdsAuditByIdAsync(inputPdsAudit.Id);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}