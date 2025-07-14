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
    public partial class ResolvedAddressMatchQualityHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            double matchRate = 1;

            double degradedThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThresholdPercentage", 0.9);

            double unHealthyThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThresholdPercentage", 0.8);

            List<ResolvedAddress> healthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset,
                percentageToMatch: matchRate);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "Match quality is good";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckDescriptionName },
                { "averageMatchRate", matchRate},
                { "isDegraded", false},
                { "isUnhealthy", false},
                { "degradedThresholdPercentage", degradedThresholdPercentage.ToString() },
                { "unHealthyThresholdPercentage", unHealthyThresholdPercentage.ToString() },
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
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            double matchRate = 0.9;

            double degradedThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThresholdPercentage", 0.9);

            double unHealthyThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThresholdPercentage", 0.8);

            List<ResolvedAddress> degradedRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset,
                percentageToMatch: matchRate);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{matchRate * 100}% average match rate. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckDescriptionName },
                { "averageMatchRate", matchRate},
                { "isDegraded", true},
                { "isUnhealthy", false},
                { "degradedThresholdPercentage", degradedThresholdPercentage.ToString() },
                { "unHealthyThresholdPercentage", unHealthyThresholdPercentage.ToString() },
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
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            double matchRate = 0.8;

            double degradedThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThresholdPercentage", 0.9);

            double unHealthyThresholdPercentage = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThresholdPercentage", 0.8);

            List<ResolvedAddress> unHealthyRecords = CreateRandomResolvedAddresses(
                dateTimeOffset: randomDateTimeOffset,
                percentageToMatch: matchRate);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{matchRate * 100}% average match rate. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckDescriptionName },
                { "averageMatchRate", matchRate},
                { "isDegraded", false},
                { "isUnhealthy", true},
                { "degradedThresholdPercentage", degradedThresholdPercentage.ToString() },
                { "unHealthyThresholdPercentage", unHealthyThresholdPercentage.ToString() },
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