// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldAddIngestionTrackingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();

            IngestionTrackingAudit randomIngestionTrackingAudit = 
                CreateRandomIngestionTrackingAudit(randomDateTimeOffset, randomEntraUser.EntraUserId);

            IngestionTrackingAudit inputIngestionTrackingAudit = randomIngestionTrackingAudit;
            IngestionTrackingAudit storageIngestionTrackingAudit = inputIngestionTrackingAudit;
            IngestionTrackingAudit expectedIngestionTrackingAudit = storageIngestionTrackingAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertIngestionTrackingAuditAsync(inputIngestionTrackingAudit))
                    .ReturnsAsync(storageIngestionTrackingAudit);

            // when
            IngestionTrackingAudit actualIngestionTrackingAudit = await this.ingestionTrackingAuditService
                .AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            // then
            actualIngestionTrackingAudit.Should().BeEquivalentTo(expectedIngestionTrackingAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(2));

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.InsertIngestionTrackingAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}