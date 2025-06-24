using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyArtifacts.FailedToProcessHealthCheck
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<TerminologyArtifact> randomUnhealthyPolls =
                CreateRandomUnhealthyTerminologyArtifacts("CodeSystem");

            IQueryable<TerminologyArtifact> randomHealthyPolls =
                CreateRandomHealthyTerminologyArtifacts("ConceptMap");

            IQueryable<TerminologyArtifact> randomPolls =
                randomUnhealthyPolls.Concat(randomHealthyPolls);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Unhealthy,
                    unHealthyCodeSystemItems: randomUnhealthyPolls.Count()
                );

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(randomPolls);

            // when
            var result = await this.terminologyArtifactsFailedToProcessHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsyncDegradedWhenAnyDegraded()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<TerminologyArtifact> randomDegradedPolls =
                CreateRandomDegradedTerminologyArtifacts("CodeSystem");

            IQueryable<TerminologyArtifact> randomHealthyPolls =
                CreateRandomHealthyTerminologyArtifacts("ValueSet");

            IQueryable<TerminologyArtifact> randomPolls =
                randomDegradedPolls.Concat(randomHealthyPolls);

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Degraded,
                    degradedCodeSystemItems: randomDegradedPolls.Count()
                );

            var expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(randomPolls);

            // when
            var result = await this.terminologyArtifactsFailedToProcessHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldGetHealthStatusAsyncHealthyWhenAllHealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;

            IQueryable<TerminologyArtifact> randomHealthyPolls =
                CreateRandomHealthyTerminologyArtifacts("CodeSystem");

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Healthy
                );

            var expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTerminologyArtifactsAsync())
                    .ReturnsAsync(randomHealthyPolls);

            // when
            var result = await this.terminologyArtifactsFailedToProcessHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTerminologyArtifactsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
