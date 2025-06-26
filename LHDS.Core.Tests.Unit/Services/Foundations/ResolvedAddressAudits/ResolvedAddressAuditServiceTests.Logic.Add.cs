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
        public async Task ShouldAddResolvedAddressAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            ResolvedAddressAudit randomResolvedAddressAudit = 
                CreateRandomResolvedAddressAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            ResolvedAddressAudit inputResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit storageResolvedAddressAudit = inputResolvedAddressAudit;
            ResolvedAddressAudit expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertResolvedAddressAuditAsync(inputResolvedAddressAudit))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ResolvedAddressAudit actualResolvedAddressAudit = await this.resolvedAddressAuditService
                .AddResolvedAddressAuditAsync(inputResolvedAddressAudit);

            // then
            actualResolvedAddressAudit.Should().BeEquivalentTo(expectedResolvedAddressAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertResolvedAddressAuditAsync(inputResolvedAddressAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}