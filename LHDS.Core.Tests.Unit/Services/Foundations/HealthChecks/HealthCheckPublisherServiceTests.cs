// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Telemetries;
using LHDS.Core.Services.Foundations.HealthChecks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherServiceTests
    {
        private readonly Mock<ITelemetryBroker> telemetryBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly HealthCheckPublisherService healthCheckPublisherService;
        private readonly CompareLogic compareLogic;

        public HealthCheckPublisherServiceTests()
        {
            this.compareLogic = new CompareLogic();
            this.telemetryBrokerMock = new Mock<ITelemetryBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.healthCheckPublisherService = new HealthCheckPublisherService(
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
                    description: $"Service {i} is healthy.",
                    duration: TimeSpan.FromMilliseconds(new Random().Next(10, 500)),
                    exception: null,
                    data: new Dictionary<string, object>
                    {
                        { "checkedAt", DateTime.UtcNow }
                    });

                entries.Add($"Service_{i}", entry);
            }

            return new HealthReport(entries, totalDuration: TimeSpan.FromSeconds(1));
        }
    }
}