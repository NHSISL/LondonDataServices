using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.Pds.ReceivedReply
{
    public partial class PdsReceivedReplyHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            Guid someCorrelationId = Guid.NewGuid();
            IQueryable<PdsAudit> randomUnhealthyPdsAudits = CreateRandomUnhealthyPdsAudits(someCorrelationId);
            IQueryable<PdsAudit> randomHealthyPdsAudits = CreateRandomHealthyPdsAudits(someCorrelationId);
            IQueryable<PdsAudit> randomTrackings = randomUnhealthyPdsAudits.Concat(randomHealthyPdsAudits);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Unhealthy,
                    unhealthyItemsCount: randomUnhealthyPdsAudits.Count());

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.pdsReceivedReplyHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
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
            Guid someCorrelationId = Guid.NewGuid();
            IQueryable<PdsAudit> randomTrackings = CreateRandomHealthyPdsAudits(someCorrelationId);

            Dictionary<string, object> healthCheckResultValues =
                 GetHealthCheckResultValues(
                     currentDateTime,
                     HealthStatus.Healthy);

            var expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.pdsReceivedReplyHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
