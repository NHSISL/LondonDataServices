using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.IngestionTrackingDecryptionHealthCheck
{
    public partial class IngestionTrackingDecryptionHealthCheckServiceTests
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
                GetHealthCheckResultValues(currentDateTime, HealthStatus.Unhealthy, unhealthyItemsCount: randomUnhealthyTrackings.Count());

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingDecryptionHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

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
                GetHealthCheckResultValues(currentDateTime, HealthStatus.Degraded, degradedItemsCount: randomDegradedTrackings.Count());

            var expectedHealthCheckResult = HealthCheckResult.Degraded(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingDecryptionHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

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
        public async Task ShouldGetHealthStatusAsyncHealthyWhenNoUnhealthyOrDegraded()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomTrackings =
                CreateRandomHealthyIngestionTrackings();

           Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(currentDateTime, HealthStatus.Healthy);

            var expectedHealthCheckResult = HealthCheckResult.Healthy(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingDecryptionHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

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
