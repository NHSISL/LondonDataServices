using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.FailedToProcessHealthCheck
{
    public partial class IngestionTrackingFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomUnhealthyTrackings =
                CreateRandomUnhealthyIngestionTrackings();

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomHealthyTrackings =
                CreateRandomHealthyIngestionTrackings();

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomTrackings =
                randomUnhealthyTrackings.Concat(randomHealthyTrackings);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime, 
                    HealthStatus.Unhealthy, 
                    unhealthyItemsCount: randomUnhealthyTrackings.Count(), 
                    healthyItemsCount: randomHealthyTrackings.Count()
                );

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingFailedToProcessHealthCheckService.GetHealthStatusAsync();

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsyncDegradedWhenAnyDegraded()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomDegradedTrackings =
                CreateRandomDegradedIngestionTrackings();

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomHealthyTrackings =
                CreateRandomHealthyIngestionTrackings();

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomTrackings =
                randomDegradedTrackings.Concat(randomHealthyTrackings);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime, 
                    HealthStatus.Degraded, 
                    degradedItemsCount: randomDegradedTrackings.Count(),
                    healthyItemsCount: randomHealthyTrackings.Count());

            var expectedHealthCheckResult = HealthCheckResult.Degraded(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingFailedToProcessHealthCheckService.GetHealthStatusAsync();

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
