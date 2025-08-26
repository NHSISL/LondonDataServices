// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        [Fact]
        public async Task ShouldReturnIngestionTrackingsAsync()
        {
            // given
            IQueryable<IngestionTracking> randomIngestionTrackings = CreateRandomIngestionTrackings();
            IQueryable<IngestionTracking> storageIngestionTrackings = randomIngestionTrackings;
            IQueryable<IngestionTracking> expectedIngestionTrackings = storageIngestionTrackings;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(storageIngestionTrackings);

            // when
            IQueryable<IngestionTracking> actualIngestionTrackings =
                await this.ingestionTrackingService.RetrieveAllIngestionTrackingsAsync();

            // then
            actualIngestionTrackings.Should().BeEquivalentTo(expectedIngestionTrackings);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}