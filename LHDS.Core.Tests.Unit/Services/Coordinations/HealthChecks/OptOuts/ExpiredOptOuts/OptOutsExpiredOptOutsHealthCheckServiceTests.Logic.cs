using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.OptOuts.ExpiredOptOuts
{
    public partial class OptOutsExpiredOptOutsHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<OptOut> randomUnhealthyTrackings =
                CreateRandomUnhealthyOptOuts();

            IQueryable<OptOut> randomHealthyTrackings =
                CreateRandomHealthyOptOuts();

            IQueryable<OptOut> randomTrackings =
                randomUnhealthyTrackings.Concat(randomHealthyTrackings);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime, HealthStatus.Unhealthy,
                    unhealthyItemsCount: randomUnhealthyTrackings.Count());

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.optOutExpiredOptOutHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(),
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

            IQueryable<OptOut> randomDegradedTrackings =
                CreateRandomDegradedOptOuts();

            IQueryable<OptOut> randomHealthyTrackings =
                CreateRandomHealthyOptOuts();

            IQueryable<OptOut> randomTrackings =
                randomDegradedTrackings.Concat(randomHealthyTrackings);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime, HealthStatus.Degraded,
                    degradedItemsCount: randomDegradedTrackings.Count());

            var expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.optOutExpiredOptOutHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(),
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

            IQueryable<OptOut> randomTrackings =
                CreateRandomHealthyOptOuts();

            Dictionary<string, object> healthCheckResultValues =
                 GetHealthCheckResultValues(currentDateTime, HealthStatus.Healthy);

            var expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.optOutExpiredOptOutHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
