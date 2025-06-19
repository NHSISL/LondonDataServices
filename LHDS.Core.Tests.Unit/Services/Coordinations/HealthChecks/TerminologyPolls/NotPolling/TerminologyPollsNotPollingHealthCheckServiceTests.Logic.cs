using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyPolls.ProcessingHealthCheck
{
    public partial class TerminologyPollsNotPollingHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<TerminologyPoll> randomUnhealthyPolls =
                CreateRandomUnhealthyTerminologyPolls();

            IQueryable<TerminologyPoll> randomHealthyPolls =
                CreateRandomHealthyTerminologyPolls();

            IQueryable<TerminologyPoll> randomPolls =
                randomUnhealthyPolls.Concat(randomHealthyPolls);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Unhealthy,
                    unhealthyItemsCount: randomUnhealthyPolls.Count()
                );

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ReturnsAsync(randomPolls);

            // when
            var result = await this.terminologyPollsNotPollingHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
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

            IQueryable<TerminologyPoll> randomDegradedPolls =
                CreateRandomDegradedTerminologyPolls();

            IQueryable<TerminologyPoll> randomHealthyPolls =
                CreateRandomHealthyTerminologyPolls();

            IQueryable<TerminologyPoll> randomPolls =
                randomDegradedPolls.Concat(randomHealthyPolls);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Degraded,
                    degradedItemsCount: randomDegradedPolls.Count()
                );

            var expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ReturnsAsync(randomPolls);

            // when
            var result = await this.terminologyPollsNotPollingHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsyncHealthyWhenAllHealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<TerminologyPoll> randomHealthyPolls =
                CreateRandomHealthyTerminologyPolls();

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Healthy
                );

            var expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyPollsAsync())
                    .ReturnsAsync(randomHealthyPolls);

            // when
            var result = await this.terminologyPollsNotPollingHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyPollsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
