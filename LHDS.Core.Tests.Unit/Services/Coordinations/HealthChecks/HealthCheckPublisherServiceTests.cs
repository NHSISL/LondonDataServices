// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Telemetries;
using LHDS.Core.Services.Foundations.HealthChecks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks
{
    public partial class HealthCheckPublisherCoordinationServiceTests
    {
        private readonly Mock<ITelemetryBroker> telemetryBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly HealthCheckPublisherCoordinationService healthCheckPublisherService;
        private readonly CompareLogic compareLogic;
        private const string KeyDelimiter = ".";

        public HealthCheckPublisherCoordinationServiceTests()
        {
            this.compareLogic = new CompareLogic();
            this.telemetryBrokerMock = new Mock<ITelemetryBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.healthCheckPublisherService = new HealthCheckPublisherCoordinationService(
                telemetryBroker: this.telemetryBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<EventTelemetry, bool>> SameEventTelemetryAs(EventTelemetry expectedEventTelemetry)
        {
            return actualEventTelemetry =>
                this.compareLogic.Compare(expectedEventTelemetry, actualEventTelemetry)
                    .AreEqual;
        }

        private Expression<Func<MetricTelemetry, bool>> SameMetricTelemetryAs(MetricTelemetry expectedMetricTelemetry)
        {
            return actualMetricTelemetry =>
                this.compareLogic.Compare(expectedMetricTelemetry, actualMetricTelemetry)
                    .AreEqual;
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        public static HealthReport CreateHealthReport(int count)
        {
            var entries = new Dictionary<string, HealthReportEntry>();

            for (int i = 0; i < count; i++)
            {
                var entry = new HealthReportEntry(
                    status: HealthStatus.Healthy,
                    description: $"Service {i} is unhealthy.",
                    duration: TimeSpan.FromMilliseconds(new Random().Next(10, 500)),
                    exception: null,
                    data: new Dictionary<string, object>
                    {
                        { GetRandomString(), GetRandomNumber() },
                        { "checkedAt", DateTime.UtcNow }
                    });

                entries.Add($"Service_{i}", entry);
            }

            return new HealthReport(entries, totalDuration: TimeSpan.FromSeconds(1));
        }

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
                        string metricKey = GetUniqueKey(telemetry.Properties, key);

                        telemetryBrokerMock.Verify(broker =>
                            broker.TrackMetricAsync(It.Is(SameMetricTelemetryAs(new MetricTelemetry(metricKey, metricValue)))),
                                Times.Once);

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