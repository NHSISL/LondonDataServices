using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.Pds.ReceivedReply
{
    public partial class PdsReceivedReplyHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            Guid someCorrelationId = Guid.NewGuid();
            IQueryable<PdsAudit> randomHealthyPdsAudits = CreateRandomHealthyPdsAudits(someCorrelationId);
            IQueryable<PdsAudit> randomTrackings = randomHealthyPdsAudits;
            string message = "All requests received reply.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "notReceivedReply", 0},
                { "unHealthyItems", 0},
                { "unHealthyThresholdMinutes", UnHealthyThresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Healthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: vals);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.pdsReceivedReplyHealthCheckService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            actualHealthCheckResult.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            actualHealthCheckResult.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            actualHealthCheckResult.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealthyAsync()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            Guid someCorrelationId = Guid.NewGuid();
            IQueryable<PdsAudit> randomUnhealthyPdsAudits = CreateRandomUnhealthyPdsAudits(someCorrelationId);
            IQueryable<PdsAudit> randomHealthyPdsAudits = CreateRandomHealthyPdsAudits(someCorrelationId);
            IQueryable<PdsAudit> randomTrackings = randomUnhealthyPdsAudits.Concat(randomHealthyPdsAudits);
            int unhealthyItemsCount = randomUnhealthyPdsAudits.Count();
            string message = $"{unhealthyItemsCount} requests have no reply. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "notReceivedReply", unhealthyItemsCount},
                { "unHealthyItems", unhealthyItemsCount},
                { "unHealthyThresholdMinutes", UnHealthyThresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: vals);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.pdsReceivedReplyHealthCheckService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            actualHealthCheckResult.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            actualHealthCheckResult.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            actualHealthCheckResult.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}