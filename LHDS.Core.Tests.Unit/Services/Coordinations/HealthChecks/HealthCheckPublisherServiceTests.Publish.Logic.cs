// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks
{
    public partial class HealthCheckPublisherCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldPublishAsync()
        {
            // given
            int randomCount = GetRandomNumber();
            HealthReport randomHealthReport = CreateHealthReport(count: randomCount);
            HealthReport inputHealthReport = randomHealthReport;

            // when
            await this.healthCheckPublisherService.PublishAsync(inputHealthReport, TestContext.Current.CancellationToken);

            // then
            double statusCode = inputHealthReport.Status switch
            {
                HealthStatus.Healthy => 2,
                HealthStatus.Degraded => 1,
                HealthStatus.Unhealthy => 0,
                _ => 0
            };

            this.telemetryBrokerMock.Verify(broker =>
                broker.TrackMetricAsync(It.Is(SameMetricTelemetryAs(new MetricTelemetry("StatusCode", statusCode)))),
                    Times.Exactly(inputHealthReport.Entries.Count));

            foreach (var entry in inputHealthReport.Entries)
            {
                var eventTelemetry = new EventTelemetry(entry.Key);
                eventTelemetry.Properties.Add("Status", entry.Value.Status.ToString());

                foreach (var reading in entry.Value.Data)
                {
                    string key = $"{entry.Key}{KeyDelimiter}{reading.Key}";
                    await AddMetricOrPropertyAsync(eventTelemetry, key, reading.Value);
                }

                this.telemetryBrokerMock.Verify(broker =>
                    broker.TrackEventAsync(It.Is(SameEventTelemetryAs(eventTelemetry))),
                        Times.Once);
            }

            this.telemetryBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}