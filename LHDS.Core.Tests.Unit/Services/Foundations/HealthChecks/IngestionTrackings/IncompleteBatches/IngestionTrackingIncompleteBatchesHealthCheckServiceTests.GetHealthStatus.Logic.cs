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
    public partial class IngestionTrackingIncompleteBatchesHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            string CheckName = "incompleteBatchesQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            string batchReference = GetRandomStringWithLengthOf(10);
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:IncompleteBatches:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:IncompleteBatches:UnHealthyThreshold", 2880);

            List<IngestionTracking> healthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset,
                batchReference: batchReference,
                isCompletedBatch: true,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "No incomplete batches. All up to date.";

            var values = new Dictionary<string, object>
            {
                { "description", "Incomplete Batches" },
                { "incompleteBatches", 0},
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
                data: values);

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
            string CheckName = "incompleteBatchesQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            string batchReference = GetRandomStringWithLengthOf(10);
            int randomNumber = GetRandomNumber();
            int distinctBatchCount = 1;

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:UnHealthyThreshold", 2880);

            List<IngestionTracking> degradedRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                batchReference: batchReference,
                isCompletedBatch: false,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{distinctBatchCount} batches incomplete. Please check logs and source locations.";

            var values = new Dictionary<string, object>
            {
                { "description", "Incomplete Batches" },
                { "incompleteBatches", distinctBatchCount},
                { "degradedItems", distinctBatchCount},
                { "unHealthyItems", 0},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Degraded.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: values);

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
        public async Task ShouldGetHealthStatusAsUnHealthyAsync()
        {
            // given
            string CheckName = "incompleteBatchesQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            string batchReference = GetRandomStringWithLengthOf(10);
            int randomNumber = GetRandomNumber();
            int distinctBatchCount = 1;

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:UnHealthyThreshold", 2880);

            List<IngestionTracking> unHealthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                batchReference: batchReference,
                isCompletedBatch: false,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{distinctBatchCount} batches incomplete. Please check logs and source locations.";

            var values = new Dictionary<string, object>
            {
                { "description", "Incomplete Batches" },
                { "incompleteBatches", distinctBatchCount},
                { "degradedItems", 0},
                { "unHealthyItems", distinctBatchCount},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: values);

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
        public async Task ShouldGetHealthStatusAsUnHealthyWithMixedItemsAsync()
        {
            // given
            string CheckName = "incompleteBatchesQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int distinctBatchCount = 2;

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:IngestionTracking:Processing:UnHealthyThreshold", 2880);

            List<IngestionTracking> healthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset,
                batchReference: GetRandomStringWithLengthOf(10),
                isCompletedBatch: true,
                count: GetRandomNumber());

            List<IngestionTracking> degradedRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                batchReference: GetRandomStringWithLengthOf(10),
                isCompletedBatch: false,
                count: GetRandomNumber());

            List<IngestionTracking> unhealthyRecords = CreateRandomIngestionTrackings(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                batchReference: GetRandomStringWithLengthOf(10),
                isCompletedBatch: false,
                count: GetRandomNumber());

            List<IngestionTracking> allRecords = [.. healthyRecords, .. degradedRecords, .. unhealthyRecords];

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message = $"{distinctBatchCount} batches incomplete. Please check logs and source locations.";

            var values = new Dictionary<string, object>
            {
                { "description", "Incomplete Batches" },
                { "incompleteBatches", distinctBatchCount},
                { "degradedItems", 1},
                { "unHealthyItems", 1},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: values);

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