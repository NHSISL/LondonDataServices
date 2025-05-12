// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings
{
    public partial class IngestionTrackingFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealtyAsync()
        {
            // given
            string CheckName = "failedToProcess";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int retryCount = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:RetryCount", 3);

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:UnHealthyThreshold", 2880);

            List<IngestionTracking> healtyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset,
                retryCount: 4,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(healtyRecords.AsQueryable());

            string message = $"{healtyRecords.Count} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", healtyRecords.Count},
                { "degradedItems", 0},
                { "unHealthyItems", 0},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Healthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Should().BeEquivalentTo(expectedHealthCheckResult, options =>
                options.Using<DateTimeOffset>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTimeOffset>()
                    .WithStrictOrdering()
                    .ComparingByMembers<HealthCheckResult>());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsDegradedAsync()
        {
            // given
            string CheckName = "failedToProcess";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int retryCount = this.inMemoryConfiguration
                 .GetValue("HealthChecks:IngestionTracking:FailedToProcess:RetryCount", 3);

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:UnHealthyThreshold", 2880);

            List<IngestionTracking> degradedRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                retryCount: 4,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{randomNumber} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", randomNumber},
                { "degradedItems", randomNumber},
                { "unHealthyItems", 0},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Degraded.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Should().BeEquivalentTo(expectedHealthCheckResult, options =>
                options.Using<DateTimeOffset>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTimeOffset>()
                    .WithStrictOrdering()
                    .ComparingByMembers<HealthCheckResult>());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealtyAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int retryCount = this.inMemoryConfiguration
                 .GetValue("HealthChecks:IngestionTracking:FailedToProcess:RetryCount", 3);

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:UnHealthyThreshold", 2880);

            List<IngestionTracking> unHealthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                retryCount: 4,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{randomNumber} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", randomNumber},
                { "degradedItems", 0},
                { "unHealthyItems", randomNumber},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Should().BeEquivalentTo(expectedHealthCheckResult, options =>
                options.Using<DateTimeOffset>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTimeOffset>()
                    .WithStrictOrdering()
                    .ComparingByMembers<HealthCheckResult>());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealtyWithMixedItemsAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;

            int retryCount = this.inMemoryConfiguration
                 .GetValue("HealthChecks:IngestionTracking:FailedToProcess:RetryCount", 3);

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:FailedToProcess:UnHealthyThreshold", 2880);

            List<IngestionTracking> healthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset,
                retryCount: 4,
                count: GetRandomNumber());

            List<IngestionTracking> degradedRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                retryCount: 4,
                count: GetRandomNumber());

            List<IngestionTracking> unhealthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                retryCount: 4,
                count: GetRandomNumber());

            List<IngestionTracking> allRecords = [.. healthyRecords, .. degradedRecords, .. unhealthyRecords];
            int unDecryptedCount = degradedRecords.Count + unhealthyRecords.Count;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message = $"{unDecryptedCount} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", unDecryptedCount},
                { "degradedItems", degradedRecords.Count},
                { "unHealthyItems", unhealthyRecords.Count},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.ingestionTrackingHealthItemService.GetHealthStatusAsync();

            // then
            actualHealthCheckResult.Should().BeEquivalentTo(expectedHealthCheckResult, options =>
                options.Using<DateTimeOffset>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                    .WhenTypeIs<DateTimeOffset>()
                    .WithStrictOrdering()
                    .ComparingByMembers<HealthCheckResult>());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}