// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldModifyResolvedAddressAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddressAudit randomResolvedAddressAudit =
                CreateRandomModifyResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit inputResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit storageResolvedAddressAudit = inputResolvedAddressAudit.DeepClone();
            storageResolvedAddressAudit.UpdatedDate = randomResolvedAddressAudit.CreatedDate;
            ResolvedAddressAudit updatedResolvedAddressAudit = inputResolvedAddressAudit;
            ResolvedAddressAudit expectedResolvedAddressAudit = updatedResolvedAddressAudit.DeepClone();
            Guid ResolvedAddressAuditId = inputResolvedAddressAudit.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(ResolvedAddressAuditId))
                    .ReturnsAsync(storageResolvedAddressAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateResolvedAddressAuditAsync(inputResolvedAddressAudit))
                    .ReturnsAsync(updatedResolvedAddressAudit);

            // when
            ResolvedAddressAudit actualResolvedAddressAudit =
                await this.resolvedAddressAuditService.ModifyResolvedAddressAuditAsync(inputResolvedAddressAudit);

            // then
            actualResolvedAddressAudit.Should().BeEquivalentTo(expectedResolvedAddressAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(inputResolvedAddressAudit.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAuditAsync(inputResolvedAddressAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}