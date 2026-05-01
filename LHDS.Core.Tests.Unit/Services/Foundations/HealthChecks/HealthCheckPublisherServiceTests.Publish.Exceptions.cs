// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.HealthChecks.Exceptions;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks
{
    public partial class HealthCheckPublisherServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            int randomCount = GetRandomNumber();
            HealthReport someHealthReport = CreateHealthReport(count: randomCount);

            var serviceException = new Exception();

            var failedHealthCheckPublisherServiceException =
                new FailedHealthCheckPublisherServiceException(
                    message: "Failed health check publisher service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedHealthCheckPublisherServiceException =
                new HealthCheckPublisherServiceException(
                    message: "Health check publisher service error occurred, please contact support.",
                    innerException: failedHealthCheckPublisherServiceException);

            this.telemetryBrokerMock.Setup(broker =>
                broker.TrackMetricAsync(It.IsAny<MetricTelemetry>()))
                    .ThrowsAsync(serviceException);

            // when
            HealthCheckPublisherServiceException actualHealthCheckPublisherServiceException =
                await Assert.ThrowsAsync<HealthCheckPublisherServiceException>(
                    () => this.healthCheckPublisherService.PublishAsync(
                        someHealthReport,
                        TestContext.Current.CancellationToken));

            // then
            actualHealthCheckPublisherServiceException.Should()
                .BeEquivalentTo(expectedHealthCheckPublisherServiceException);

            this.telemetryBrokerMock.Verify(broker =>
                broker.TrackMetricAsync(It.IsAny<MetricTelemetry>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedHealthCheckPublisherServiceException))),
                        Times.Once);

            this.telemetryBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}