// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Telemetries;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks
{
    public class HealthCheckPublisherService : IHealthCheckPublisher
    {
        private readonly ITelemetryBroker telemetryBroker;

        public HealthCheckPublisherService(ITelemetryBroker telemetryBroker)
        {
            this.telemetryBroker = telemetryBroker;
        }

        public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            foreach (var entry in report.Entries)
            {
                var eventTelemetry = new EventTelemetry(entry.Key);
                eventTelemetry.Properties.Add("Status", report.Status.ToString());

                foreach (var reading in entry.Value.Data)
                {
                    var metric = new MetricTelemetry();
                    metric.Name = reading.Key;
                    metric.Sum = double.Parse(reading.Value.ToString());
                    eventTelemetry.Metrics.Add(metric.Name, metric.Sum);
                    await telemetryBroker.TrackMetricAsync(metric);
                }

                await telemetryBroker.TrackEventAsync(eventTelemetry);
            }
        }
    }
}
