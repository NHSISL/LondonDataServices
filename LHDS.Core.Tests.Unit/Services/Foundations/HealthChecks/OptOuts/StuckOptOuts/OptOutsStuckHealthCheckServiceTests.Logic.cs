// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.StuckOptOuts
{
    public partial class OptOutsStuckHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> healthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "Nothing is stuck. All opt outs are being sent to MESH.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", 0 },
                { "degradedItems", 0 },
                { "unHealthyItems", 0 },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsDegradedAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> degradedRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message =
                $"{randomNumber} opt outs are active but have not been sent to MESH recently. " +
                "Please check logs and function status.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", randomNumber },
                { "degradedItems", randomNumber },
                { "unHealthyItems", 0 },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealthyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> unHealthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message =
                $"{randomNumber} opt outs are active but have not been sent to MESH recently. " +
                "Please check logs and function status.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", randomNumber },
                { "degradedItems", 0 },
                { "unHealthyItems", randomNumber },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsUnHealthyWithMixedItemsAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> healthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset,
                count: GetRandomNumber());

            List<OptOut> degradedRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                count: GetRandomNumber());

            List<OptOut> unHealthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: GetRandomNumber());

            List<OptOut> allRecords = [.. healthyRecords, .. degradedRecords, .. unHealthyRecords];
            int totalCount = degradedRecords.Count + unHealthyRecords.Count;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message =
                $"{totalCount} opt outs are active but have not been sent to MESH recently. " +
                "Please check logs and function status.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", totalCount },
                { "degradedItems", degradedRecords.Count },
                { "unHealthyItems", unHealthyRecords.Count },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldExcludeExpiredOptOutsFromCountAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> expiredStuckRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: randomNumber,
                isExpired: true);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(expiredStuckRecords.AsQueryable());

            string message = "Nothing is stuck. All opt outs are being sent to MESH.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", 0 },
                { "degradedItems", 0 },
                { "unHealthyItems", 0 },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldExcludeRecentlySentOptOutsFromCountAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes =
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<OptOut> recentlySentRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: randomNumber,
                isRecentlySent: true);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(recentlySentRecords.AsQueryable());

            string message = "Nothing is stuck. All opt outs are being sent to MESH.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "stuckOptOuts", 0 },
                { "degradedItems", 0 },
                { "unHealthyItems", 0 },
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
                await this.optOutsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllOptOutsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
