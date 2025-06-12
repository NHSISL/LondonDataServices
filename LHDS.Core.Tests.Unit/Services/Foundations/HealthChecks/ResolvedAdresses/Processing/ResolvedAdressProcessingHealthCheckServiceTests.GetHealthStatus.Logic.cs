// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses
{
    public partial class ResolvedAdressProcessingHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:UnHealthyThreshold", 2880);

            List<ResolvedAddress> healthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset,
                isProcessing: true,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "Nothing to process. All up to date.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Processing Queue" },
                { "stuckInProcessing", 0},
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
                await this.resolvedAddressHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsDegradedAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:UnHealthyThreshold", 2880);

            List<ResolvedAddress> degradedRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                isProcessing: true,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{randomNumber} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Processing Queue" },
                { "stuckInProcessing", randomNumber},
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
                await this.resolvedAddressHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealthyAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:UnHealthyThreshold", 2880);

            List<ResolvedAddress> unHealthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                isProcessing: true,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{randomNumber} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Processing Queue" },
                { "stuckInProcessing", randomNumber},
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
                await this.resolvedAddressHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealthyWithMixedItemsAsync()
        {
            // given
            string CheckName = "processingQueue";
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue("HealthChecks:ResolvedAddress:Processing:UnHealthyThreshold", 2880);

            List<ResolvedAddress> healthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset,
                isProcessing: true,
                count: GetRandomNumber());

            List<ResolvedAddress> degradedRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                isProcessing: true,
                count: GetRandomNumber());

            List<ResolvedAddress> unhealthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                isProcessing: true,
                count: GetRandomNumber());

            List<ResolvedAddress> allRecords = [.. healthyRecords, .. degradedRecords, .. unhealthyRecords];
            int unDecryptedCount = degradedRecords.Count + unhealthyRecords.Count;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message = $"{unDecryptedCount} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Processing Queue" },
                { "stuckInProcessing", unDecryptedCount},
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
                await this.resolvedAddressHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}