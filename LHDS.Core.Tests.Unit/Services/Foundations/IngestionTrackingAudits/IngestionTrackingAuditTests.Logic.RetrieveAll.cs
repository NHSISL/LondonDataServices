// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public void ShouldReturnAudits()
        {
            // given
            IQueryable<IngestionTrackingAudit> randomIngestionTrackingAudits = CreateRandomIngestionTrackingAudits();
            IQueryable<IngestionTrackingAudit> storageIngestionTrackingAudits = randomIngestionTrackingAudits;
            IQueryable<IngestionTrackingAudit> expectedIngestionTrackingAudits = storageIngestionTrackingAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync())
                    .Returns(storageIngestionTrackingAudits);

            // when
            IQueryable<IngestionTrackingAudit> actualIngestionTrackingAudits =
                this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync(;

            // then
            actualIngestionTrackingAudits.Should().BeEquivalentTo(expectedIngestionTrackingAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}