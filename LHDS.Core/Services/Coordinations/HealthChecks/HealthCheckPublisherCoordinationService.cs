// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Telemetries;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherCoordinationService : IHealthCheckPublisher
    {
        private readonly ITelemetryBroker telemetryBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string KeyDelimiter = ".";
        private const int HealthyStatusCode = 2;
        private const int DegradedStatusCode = 1;
        private const int UnhealthyStatusCode = 0;
        private const int UnknownStatusCode = 0;

        public HealthCheckPublisherCoordinationService(ITelemetryBroker telemetryBroker, ILoggingBroker loggingBroker)
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
                eventTelemetry.Properties.Add("Status", entry.Value.Status.ToString());

                double statusCode = entry.Value.Status switch
                {
                    HealthStatus.Healthy => HealthyStatusCode,
                    HealthStatus.Degraded => DegradedStatusCode,
                    HealthStatus.Unhealthy => UnhealthyStatusCode,
                    _ => UnknownStatusCode
                };

                await telemetryBroker.TrackMetricAsync(new MetricTelemetry("StatusCode", statusCode));

                foreach (var reading in entry.Value.Data)
                {
                    string key = $"{entry.Key}{KeyDelimiter}{reading.Key}";
                    await AddMetricOrPropertyAsync(eventTelemetry, key, reading.Value);
                }

                await telemetryBroker.TrackEventAsync(eventTelemetry);
            }
        });

        private async ValueTask AddMetricOrPropertyAsync(EventTelemetry telemetry, string key, object value)
        {
            if (value is IDictionary<string, object> nestedDict)
            {
                foreach (var nested in nestedDict)
                {
                    string nestedKey = $"{key}{KeyDelimiter}{nested.Key}";
                    await AddMetricOrPropertyAsync(telemetry, nestedKey, nested.Value);
                }
            }
            else
            {
                switch (value)
                {
                    case int or long or float or double or decimal:
                        double metricValue = Convert.ToDouble(value);
                        string metricKey = GetUniqueKey(new Dictionary<string, double>(), key);
                        await telemetryBroker.TrackMetricAsync(new MetricTelemetry(metricKey, metricValue));
                        break;

                    case DateTime dateTime:
                        string dtKey = GetUniqueKey(telemetry.Properties, key);
                        telemetry.Properties.Add(dtKey, dateTime.ToString("o"));
                        break;

                    case DateTimeOffset dateTimeOffset:
                        string dtoKey = GetUniqueKey(telemetry.Properties, key);
                        telemetry.Properties.Add(dtoKey, dateTimeOffset.ToString("o"));
                        break;

                    default:
                        string propKey = GetUniqueKey(telemetry.Properties, key);
                        telemetry.Properties.Add(propKey, value?.ToString());
                        break;
                }
            }
        }

        private string GetUniqueKey<TValue>(IDictionary<string, TValue> dictionary, string baseKey)
        {
            int suffix = 1;
            string key = baseKey;

            while (dictionary.ContainsKey(key))
            {
                key = $"{baseKey} ({suffix++})";
            }

            return key;
        }
    }
}
