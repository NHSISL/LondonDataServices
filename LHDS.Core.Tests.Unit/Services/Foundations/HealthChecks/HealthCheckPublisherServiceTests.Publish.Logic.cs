// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherServiceTests
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
                HealthStatus.Healthy => 0,
                HealthStatus.Degraded => 1,
                HealthStatus.Unhealthy => 2,
                _ => 3
            };

            this.telemetryBrokerMock.Verify(broker =>
                broker.TrackMetricAsync(It.Is(SameMetricTelemetryAs(new MetricTelemetry("StatusCode", statusCode)))),
                    Times.Exactly(inputHealthReport.Entries.Count));

            foreach (var entry in inputHealthReport.Entries)
            {
                var eventTelemetry = new EventTelemetry(entry.Key);
                eventTelemetry.Properties.Add("Status", inputHealthReport.Status.ToString());

                foreach (var reading in entry.Value.Data)
                {
                    if (reading.Value is int or long or float or double or decimal)
                    {
                        double metricValue = Convert.ToDouble(reading.Value);
                        var metric = new MetricTelemetry(reading.Key, metricValue);

                        this.telemetryBrokerMock.Verify(broker =>
                            broker.TrackMetricAsync(It.Is(SameMetricTelemetryAs(metric))),
                                Times.Once);
                    }
                    else if (reading.Value is DateTime dateTime)
                    {
                        eventTelemetry.Properties.Add(reading.Key, dateTime.ToString("o"));
                    }
                    else if (reading.Value is DateTimeOffset dateTimeOffset)
                    {
                        eventTelemetry.Properties.Add(reading.Key, dateTimeOffset.ToString("o"));
                    }
                    else
                    {
                        eventTelemetry.Properties.Add(reading.Key, reading.Value?.ToString());
                    }
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