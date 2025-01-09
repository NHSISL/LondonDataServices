// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditTests
    {
        [Fact]
        public async Task ShouldReturnAuditsAsync()
        {
            // given
            IQueryable<IngestionTrackingAudit> randomIngestionTrackingAudits = CreateRandomIngestionTrackingAudits();
            IQueryable<IngestionTrackingAudit> storageIngestionTrackingAudits = randomIngestionTrackingAudits;
            IQueryable<IngestionTrackingAudit> expectedIngestionTrackingAudits = storageIngestionTrackingAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingAuditsAsync())
                    .ReturnsAsync(storageIngestionTrackingAudits);

            // when
            IQueryable<IngestionTrackingAudit> actualIngestionTrackingAudits =
                await this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync();

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