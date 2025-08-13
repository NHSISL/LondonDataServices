using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.FailedToProcess
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<TerminologyArtifact> healthyRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset,
                resourceType: "CodeSystem",
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(healthyRecords.AsQueryable());

            string message = "Nothing to process. All up to date.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "failedToProcess", 0 },
                { "degradedCodeSystemItems", 0 },
                { "unHealthyCodeSystemItems", 0 },
                { "degradedConceptMapItems", 0 },
                { "unHealthyConceptMapItems", 0 },
                { "degradedValueSetItems", 0 },
                { "unHealthyValueItems", 0 },
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
                await this.terminologyPollsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllTerminologyArtifactsAsync(),
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

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<TerminologyArtifact> degradedRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                resourceType: "CodeSystem",
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(degradedRecords.AsQueryable());

            string message = $"{randomNumber} have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "failedToProcess", degradedRecords.Count },
                { "degradedCodeSystemItems", degradedRecords.Count },
                { "unHealthyCodeSystemItems", 0 },
                { "degradedConceptMapItems", 0 },
                { "unHealthyConceptMapItems", 0 },
                { "degradedValueSetItems", 0 },
                { "unHealthyValueItems", 0 },
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
                await this.terminologyPollsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllTerminologyArtifactsAsync(),
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

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<TerminologyArtifact> unHealthyRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                resourceType: "CodeSystem",
                count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(unHealthyRecords.AsQueryable());

            string message = $"{randomNumber} have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "failedToProcess", unHealthyRecords.Count },
                { "degradedCodeSystemItems", 0 },
                { "unHealthyCodeSystemItems", unHealthyRecords.Count },
                { "degradedConceptMapItems", 0 },
                { "unHealthyConceptMapItems", 0 },
                { "degradedValueSetItems", 0 },
                { "unHealthyValueItems", 0 },
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
                await this.terminologyPollsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllTerminologyArtifactsAsync(),
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

            int degradedThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = this.inMemoryConfiguration
                .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            List<TerminologyArtifact> healthyRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset,
                resourceType: "CodeSystem",
                count: GetRandomNumber());

            List<TerminologyArtifact> degradedRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-degradedThresholdMinutes).AddSeconds(-1),
                resourceType: "ConceptMap",
                count: GetRandomNumber());

            List<TerminologyArtifact> unhealthyRecords = CreateRandomTerminologyArtifacts(
                dateTimeOffset: randomDateTimeOffset.AddMinutes(-unHealthyThresholdMinutes).AddSeconds(-1),
                resourceType: "ValueSet",
                count: GetRandomNumber());

            List<TerminologyArtifact> allRecords = [.. healthyRecords, .. degradedRecords, .. unhealthyRecords];
            int nonHealthyCount = degradedRecords.Count + unhealthyRecords.Count;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(allRecords.AsQueryable());

            string message = $"{nonHealthyCount} have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "failedToProcess", nonHealthyCount },
                { "degradedCodeSystemItems", 0 },
                { "unHealthyCodeSystemItems", 0 },
                { "degradedConceptMapItems", degradedRecords.Count },
                { "unHealthyConceptMapItems", 0 },
                { "degradedValueSetItems", 0 },
                { "unHealthyValueItems", unhealthyRecords.Count },
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
                await this.terminologyPollsHealthItemService.GetHealthStatusAsync();

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
                broker.SelectAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
