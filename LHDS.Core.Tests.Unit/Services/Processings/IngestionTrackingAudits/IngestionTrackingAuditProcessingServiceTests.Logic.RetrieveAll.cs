// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Audits;
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
            IQueryable<Audit> randomIngestionTrackings = CreateRandomIngestionTrackingAudits();
            IQueryable<Audit> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<Audit> expectedIngestionTrackings = storageIngestionTrackings;

            this.ingestionTrackingAuditServiceMock.Setup(broker =>
                broker.RetrieveAllAudits())
                    .Returns(storageIngestionTrackings);

            // when
            IQueryable<Audit> actualIngestionTrackingAudits =
                this.ingestionTrackingAuditProcessingService.RetrieveAllIngestionTrackingAudits();

            // then
            actualIngestionTrackingAudits.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.ingestionTrackingAuditServiceMock.Verify(broker =>
                broker.RetrieveAllAudits(),
                    Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
