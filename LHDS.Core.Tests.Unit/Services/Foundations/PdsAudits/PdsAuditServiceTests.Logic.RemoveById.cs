// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldRemovePdsAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputPdsAuditId = randomId;
            PdsAudit randomPdsAudit = CreateRandomPdsAudit();
            PdsAudit storagePdsAudit = randomPdsAudit;
            PdsAudit expectedInputPdsAudit = storagePdsAudit;
            PdsAudit deletedPdsAudit = expectedInputPdsAudit;
            PdsAudit expectedPdsAudit = deletedPdsAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAuditId))
                    .ReturnsAsync(storagePdsAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeletePdsAuditAsync(expectedInputPdsAudit))
                    .ReturnsAsync(deletedPdsAudit);

            // when
            PdsAudit actualPdsAudit = await this.pdsAuditService
                .RemovePdsAuditByIdAsync(inputPdsAuditId);

            // then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectPdsAuditByIdAsync(inputPdsAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeletePdsAuditAsync(expectedInputPdsAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}