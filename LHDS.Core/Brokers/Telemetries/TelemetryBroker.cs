// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Brokers.Telemetries
{
    public class TelemetryBroker : ITelemetryBroker
    {
        private readonly TelemetryClient telemetryClient;

        public TelemetryBroker(IConfiguration configuration)
        {
            string connectionString = configuration["ApplicationInsights:ConnectionString"];
            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = connectionString;
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        /// <summary>
        /// Tracks a custom event.
        /// </summary>
        /// <param name="eventName">A name for the event.</param>
        /// <param name="properties">Optional custom properties.</param>
        /// <param name="metrics">Optional custom metrics.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackEventAsync(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.telemetryClient.TrackEvent(eventName, properties);
        }

        /// <summary>
        /// Tracks a custom event using an EventTelemetry instance.
        /// </summary>
        /// <param name="telemetry">The telemetry event to track.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackEventAsync(EventTelemetry telemetry) =>
            this.telemetryClient.TrackEvent(telemetry);

        /// <summary>
        /// Tracks a trace message.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackTraceAsync(string message) =>
            this.telemetryClient.TrackTrace(message);

        /// <summary>
        /// Tracks a trace message with a severity level.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        /// <param name="severityLevel">The severity level of the trace.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackTraceAsync(string message, SeverityLevel severityLevel) =>
            this.telemetryClient.TrackTrace(message, severityLevel);

        /// <summary>
        /// Tracks a trace message with properties.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        /// <param name="properties">Custom properties associated with the trace.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackTraceAsync(string message, IDictionary<string, string> properties) =>
            this.telemetryClient.TrackTrace(message, properties);

        /// <summary>
        /// Tracks a trace message with severity level and properties.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        /// <param name="severityLevel">The severity level of the trace.</param>
        /// <param name="properties">Custom properties associated with the trace.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackTraceAsync(
            string message,
            SeverityLevel severityLevel,
            IDictionary<string, string> properties) =>
                this.telemetryClient.TrackTrace(message, severityLevel, properties);

        /// <summary>
        /// Tracks a trace using a TraceTelemetry instance.
        /// </summary>
        /// <param name="telemetry">The telemetry trace to track.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackTraceAsync(TraceTelemetry telemetry) =>
            this.telemetryClient.TrackTrace(telemetry);

        /// <summary>
        /// Tracks a metric value.
        /// </summary>
        /// <param name="name">The name of the metric.</param>
        /// <param name="value">The value of the metric.</param>
        /// <param name="properties">Optional properties for the metric.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackMetricAsync(
            string name,
            double value,
            IDictionary<string, string> properties = null) =>
                this.telemetryClient.TrackMetric(name, value, properties);

        /// <summary>
        /// Tracks a metric using a MetricTelemetry instance.
        /// </summary>
        /// <param name="telemetry">The telemetry metric to track.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackMetricAsync(MetricTelemetry telemetry) =>
            this.telemetryClient.TrackMetric(telemetry);

        /// <summary>
        /// Tracks an exception.
        /// </summary>
        /// <param name="exception">The exception to track.</param>
        /// <param name="properties">Optional properties for classification.</param>
        /// <param name="metrics">Optional metrics associated with the exception.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackExceptionAsync(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null) =>
                this.telemetryClient.TrackException(exception, properties);

        /// <summary>
        /// Tracks an exception using an ExceptionTelemetry instance.
        /// </summary>
        /// <param name="telemetry">The telemetry exception to track.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackExceptionAsync(ExceptionTelemetry telemetry) =>
            this.telemetryClient.TrackException(telemetry);

        /// <summary>
        /// Tracks a dependency call.
        /// </summary>
        /// <param name="dependencyTypeName">The type of dependency.</param>
        /// <param name="dependencyName">The name of the dependency.</param>
        /// <param name="data">The command or data involved in the call.</param>
        /// <param name="startTime">When the call started.</param>
        /// <param name="duration">How long the call took.</param>
        /// <param name="success">Whether the call was successful.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackDependencyAsync(
            string dependencyTypeName,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success)
        {
            this.telemetryClient.TrackDependency(
                dependencyTypeName,
                dependencyName,
                data,
                startTime,
                duration,
                success);
        }

        /// <summary>
        /// Tracks a dependency call with more context.
        /// </summary>
        /// <param name="dependencyTypeName">The type of dependency.</param>
        /// <param name="target">The target of the dependency.</param>
        /// <param name="dependencyName">The name of the dependency.</param>
        /// <param name="data">The command or data involved in the call.</param>
        /// <param name="startTime">When the call started.</param>
        /// <param name="duration">How long the call took.</param>
        /// <param name="resultCode">The result code of the call.</param>
        /// <param name="success">Whether the call was successful.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackDependencyAsync(
            string dependencyTypeName,
            string target,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            string resultCode,
            bool success)
        {
            this.telemetryClient.TrackDependency(
                dependencyTypeName,
                target,
                dependencyName,
                data,
                startTime,
                duration,
                resultCode,
                success);
        }

        /// <summary>
        /// Tracks a dependency using a DependencyTelemetry instance.
        /// </summary>
        /// <param name="telemetry">The telemetry dependency to track.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask TrackDependencyAsync(DependencyTelemetry telemetry) =>
            this.telemetryClient.TrackDependency(telemetry);

        /// <summary>
        /// Flushes all pending telemetry data.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the flush operation.</param>
        /// <returns>
        /// A task that represents the asynchronous flush operation. 
        /// The result indicates whether the flush was successful.
        /// </returns>
        public async ValueTask<bool> FlushAsync(CancellationToken cancellationToken) =>
            await this.telemetryClient.FlushAsync(cancellationToken);

        /// <summary>
        /// Gets or creates a metric container by ID.
        /// </summary>
        /// <param name="metricId">The ID (name) of the metric.</param>
        /// <returns>A Metric with the specified ID.</returns>
        public async ValueTask<Metric> GetMetricAsync(string metricId) =>
            this.telemetryClient.GetMetric(metricId);

        /// <summary>
        /// Gets or creates a metric container by ID with a single dimension.
        /// </summary>
        /// <param name="metricId">The metric ID.</param>
        /// <param name="dimension1Name">The name of the first dimension.</param>
        /// <returns>A Metric with one dimension.</returns>
        public async ValueTask<Metric> GetMetricAsync(
            string metricId,
            string dimension1Name) =>
                this.telemetryClient.GetMetric(metricId, dimension1Name);

        /// <summary>
        /// Gets or creates a metric container with two dimensions.
        /// </summary>
        /// <param name="metricId">Metric ID.</param>
        /// <param name="dimension1Name">First dimension name.</param>
        /// <param name="dimension2Name">Second dimension name.</param>
        /// <returns>A Metric with two dimensions.</returns>
        public async ValueTask<Metric> GetMetricAsync(
            string metricId,
            string dimension1Name,
            string dimension2Name) =>
                this.telemetryClient.GetMetric(metricId, dimension1Name, dimension2Name);

        /// <summary>
        /// Gets or creates a metric using a MetricIdentifier.
        /// </summary>
        /// <param name="metricIdentifier">The metric identifier object.</param>
        /// <returns>The metric object.</returns>
        public async ValueTask<Metric> GetMetricAsync(MetricIdentifier metricIdentifier) =>
            this.telemetryClient.GetMetric(metricIdentifier);
    }
}
