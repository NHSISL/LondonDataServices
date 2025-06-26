// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldRemoveResolvedAddressAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputResolvedAddressAuditId = randomId;
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            ResolvedAddressAudit storageResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit expectedInputResolvedAddressAudit = storageResolvedAddressAudit;
            ResolvedAddressAudit deletedResolvedAddressAudit = expectedInputResolvedAddressAudit;
            ResolvedAddressAudit expectedResolvedAddressAudit = deletedResolvedAddressAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(inputResolvedAddressAuditId))
                    .ReturnsAsync(storageResolvedAddressAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteResolvedAddressAuditAsync(expectedInputResolvedAddressAudit))
                    .ReturnsAsync(deletedResolvedAddressAudit);

            // when
            ResolvedAddressAudit actualResolvedAddressAudit = await this.resolvedAddressAuditService
                .RemoveResolvedAddressAuditByIdAsync(inputResolvedAddressAuditId);

            // then
            actualResolvedAddressAudit.Should().BeEquivalentTo(expectedResolvedAddressAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(inputResolvedAddressAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteResolvedAddressAuditAsync(expectedInputResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}