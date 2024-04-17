// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Audits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Audits
{
    public partial class AuditServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAuditId = randomId;
            Audit randomAudit = CreateRandomAudit();
            Audit storageAudit = randomAudit;
            Audit expectedInputAudit = storageAudit;
            Audit deletedAudit = expectedInputAudit;
            Audit expectedAudit = deletedAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAuditByIdAsync(inputAuditId))
                    .ReturnsAsync(storageAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAuditAsync(expectedInputAudit))
                    .ReturnsAsync(deletedAudit);

            // when
            Audit actualAudit = await this.auditService
                .RemoveAuditByIdAsync(inputAuditId);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAuditByIdAsync(inputAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAuditAsync(expectedInputAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}