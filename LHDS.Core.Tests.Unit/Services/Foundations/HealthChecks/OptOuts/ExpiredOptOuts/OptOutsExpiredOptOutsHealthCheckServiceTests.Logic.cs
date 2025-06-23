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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.ExpiredOptOuts
{
    public partial class OptOutsExpiredOptOutsHealthCheckServiceTests
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

            int expiredAfterDays = this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

            int lastSentExpiredAfterDays = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:LastSentExpiredAfterDays", 2);

            List<OptOut> healthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset,
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "Nothing is expired and outdated. All up to date.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "expiredAndOutdated", 0},
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
                await this.OptOutsHealthItemService.GetHealthStatusAsync();

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

            int expiredAfterDays = this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

            int lastSentExpiredAfterDays = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:LastSentExpiredAfterDays", 2);

            List<OptOut> degradedRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{randomNumber} opt outs expired and outdated. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "expiredAndOutdated", randomNumber},
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
                await this.OptOutsHealthItemService.GetHealthStatusAsync();

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

            int expiredAfterDays = this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

            int lastSentExpiredAfterDays = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:LastSentExpiredAfterDays", 2);

            List<OptOut> unHealthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{randomNumber} opt outs expired and outdated. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "expiredAndOutdated", randomNumber},
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
                await this.OptOutsHealthItemService.GetHealthStatusAsync();

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
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            int expiredAfterDays = this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

            int lastSentExpiredAfterDays = 
                this.inMemoryConfiguration.GetValue($"{ConfigSectionName}:LastSentExpiredAfterDays", 2);

            List<OptOut> healthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset,
                count: GetRandomNumber());

            List<OptOut> degradedRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                count: GetRandomNumber());

            List<OptOut> unhealthyRecords = CreateRandomOptOuts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                count: GetRandomNumber());

            List<OptOut> allRecords = [.. healthyRecords, .. degradedRecords, .. unhealthyRecords];
            int expiredAndOutdatedCount = degradedRecords.Count + unhealthyRecords.Count;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllOptOutsAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message = 
                $"{expiredAndOutdatedCount} opt outs expired and outdated. " + 
                "Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "expiredAndOutdated", expiredAndOutdatedCount},
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
                await this.OptOutsHealthItemService.GetHealthStatusAsync();

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
