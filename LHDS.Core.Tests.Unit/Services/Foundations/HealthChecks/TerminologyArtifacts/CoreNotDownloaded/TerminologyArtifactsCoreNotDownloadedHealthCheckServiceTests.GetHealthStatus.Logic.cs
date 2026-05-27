// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.CoreNotDownloaded
{
    public partial class TerminologyArtifactsCoreNotDownloadedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsHealthyWhenAllCoreArtifactsDownloadedAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            var downloadedArtifacts = new List<TerminologyArtifact>();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(downloadedArtifacts.AsQueryable());

            string message = "All core terminology artifacts have been downloaded.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "coreNotDownloaded", 0 },
                { "codeSystemItems", 0 },
                { "conceptMapItems", 0 },
                { "valueSetItems", 0 },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Healthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.terminologyArtifactsHealthItemService.GetHealthStatusAsync();

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
        public async Task ShouldGetHealthStatusAsUnhealthyWhenCoreCodeSystemNotDownloadedAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            List<TerminologyArtifact> coreNotDownloaded =
                CreateRandomCoreNotDownloadedTerminologyArtifacts(
                    dateTimeOffset: randomDateTimeOffset,
                    resourceType: "CodeSystem",
                    count: randomNumber);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(coreNotDownloaded.AsQueryable());

            string message = $"{randomNumber} core terminology artifacts have not been downloaded. " +
                $"Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "coreNotDownloaded", randomNumber },
                { "codeSystemItems", randomNumber },
                { "conceptMapItems", 0 },
                { "valueSetItems", 0 },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.terminologyArtifactsHealthItemService.GetHealthStatusAsync();

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
        public async Task ShouldExcludeNonCoreArtifactsFromCoreNotDownloadedCountAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = DateTimeOffset.UtcNow;
            int randomNumber = GetRandomNumber();

            var nonCoreNotDownloaded = Enumerable.Range(0, randomNumber)
                .Select(_ => new TerminologyArtifact
                {
                    IsCore = false,
                    IsDownloaded = false,
                    IsError = false,
                    ResourceType = "CodeSystem",
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedBy = Guid.NewGuid().ToString(),
                    UpdatedDate = randomDateTimeOffset,
                    CreatedDate = randomDateTimeOffset
                })
                .ToList();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(nonCoreNotDownloaded.AsQueryable());

            string message = "All core terminology artifacts have been downloaded.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "coreNotDownloaded", 0 },
                { "codeSystemItems", 0 },
                { "conceptMapItems", 0 },
                { "valueSetItems", 0 },
                { "checkedAt", randomDateTimeOffset.ToString("o") },
                { "message", message },
                { "status", HealthStatus.Healthy.ToString() }
            };

            HealthCheckResult expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: vals);

            // when
            HealthCheckResult actualHealthCheckResult =
                await this.terminologyArtifactsHealthItemService.GetHealthStatusAsync();

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
