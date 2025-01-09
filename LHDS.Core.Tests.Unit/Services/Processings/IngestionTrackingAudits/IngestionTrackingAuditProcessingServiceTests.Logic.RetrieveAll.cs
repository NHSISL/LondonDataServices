// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllIngestionTrackings()
        {
            // given
            IQueryable<IngestionTrackingAudit> randomIngestionTrackings = CreateRandomIngestionTrackingAudits();
            IQueryable<IngestionTrackingAudit> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<IngestionTrackingAudit> expectedIngestionTrackings = storageIngestionTrackings;

            this.ingestionTrackingAuditServiceMock.Setup(broker =>
                broker.RetrieveAllIngestionTrackingAuditsAsync()
                    .Returns(storageIngestionTrackings);

            // when
            IQueryable<IngestionTrackingAudit> actualIngestionTrackingAudits =
                this.ingestionTrackingAuditProcessingService.RetrieveAllIngestionTrackingAuditsAsync(;

            // then
            actualIngestionTrackingAudits.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.ingestionTrackingAuditServiceMock.Verify(broker =>
                broker.RetrieveAllIngestionTrackingAuditsAsync(,
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
