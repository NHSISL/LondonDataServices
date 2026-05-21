// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Telemetries;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherService : IHealthCheckPublisher
    {
        private readonly ITelemetryBroker telemetryBroker;
        private readonly ILoggingBroker loggingBroker;

        public HealthCheckPublisherService(ITelemetryBroker telemetryBroker, ILoggingBroker loggingBroker)
        {
            this.telemetryBroker = telemetryBroker;
            this.loggingBroker = loggingBroker;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            foreach (var entry in report.Entries)
            {
                var eventTelemetry = new EventTelemetry(entry.Key);
                eventTelemetry.Properties.Add("Status", report.Status.ToString());

                double statusCode = report.Status switch
                {
                    HealthStatus.Healthy => 0,
                    HealthStatus.Degraded => 1,
                    HealthStatus.Unhealthy => 2,
                    _ => 3
                };

                await telemetryBroker.TrackMetricAsync(new MetricTelemetry("StatusCode", statusCode));

                foreach (var reading in entry.Value.Data)
                {
                    if (reading.Value is int or long or float or double or decimal)
                    {
                        double metricValue = Convert.ToDouble(reading.Value);
                        var metric = new MetricTelemetry(reading.Key, metricValue);
                        await telemetryBroker.TrackMetricAsync(metric);
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

                await telemetryBroker.TrackEventAsync(eventTelemetry);
            }
        });
    }
}
