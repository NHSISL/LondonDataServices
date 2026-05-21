using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTrackings.FilesReceivedHealthCheck
{
    public partial class IngestionTrackingFilesReceivedHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            IQueryable<Supplier> randomIngestionTrackedSuppliers = CreateRandomIngestionTrackedSupplier();
            Supplier randomSupplier = randomIngestionTrackedSuppliers.FirstOrDefault();

            IQueryable<IngestionTracking> randomUnhealthyTrackings =
                CreateRandomUnhealthyIngestionTrackings(randomSupplier.Id);

            int expectedItemsCount = randomUnhealthyTrackings.Count();

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Unhealthy,
                    randomSupplier.Name,
                    expectedItemsCount: expectedItemsCount
                );

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .ReturnsAsync(randomIngestionTrackedSuppliers);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id))
                    .ReturnsAsync(randomSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomUnhealthyTrackings);

            // when
            var result = await this.ingestionTrackingFilesReceivedHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
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
            IQueryable<Supplier> randomIngestionTrackedSuppliers = CreateRandomIngestionTrackedSupplier();
            Supplier randomSupplier = randomIngestionTrackedSuppliers.FirstOrDefault();

            IQueryable<IngestionTracking> randomDegradedTrackings =
                CreateRandomDegradedIngestionTrackings(randomSupplier.Id);

            int expectedItemsCount = randomDegradedTrackings.Count();

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Degraded,
                    randomSupplier.Name,
                    expectedItemsCount: expectedItemsCount
                );

            var expectedHealthCheckResult = HealthCheckResult.Degraded(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .ReturnsAsync(randomIngestionTrackedSuppliers);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id))
                    .ReturnsAsync(randomSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomDegradedTrackings);

            // when
            var result = await this.ingestionTrackingFilesReceivedHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
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
            IQueryable<Supplier> randomIngestionTrackedSuppliers = CreateRandomIngestionTrackedSupplier();
            Supplier randomSupplier = randomIngestionTrackedSuppliers.FirstOrDefault();

            IQueryable<IngestionTracking> randomHealthyTrackings =
                CreateRandomHealthyIngestionTrackings(randomSupplier.Id);

            int expectedItemsCount = randomHealthyTrackings.Count();

            Dictionary<string, object> healthCheckResultValues =
                GetHealthCheckResultValues(
                    currentDateTime,
                    HealthStatus.Healthy,
                    randomSupplier.Name,
                    expectedItemsCount: expectedItemsCount
                );

            var expectedHealthCheckResult = HealthCheckResult.Healthy(
                description: CheckName,
                data: healthCheckResultValues
            );

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSuppliersAsync())
                    .ReturnsAsync(randomIngestionTrackedSuppliers);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id))
                    .ReturnsAsync(randomSupplier);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomHealthyTrackings);

            // when
            var result = await this.ingestionTrackingFilesReceivedHealthCheckService.GetHealthStatusAsync();

            // then
            result.Data.Should().BeEquivalentTo(expectedHealthCheckResult.Data);
            result.Description.Should().BeEquivalentTo(expectedHealthCheckResult.Description);
            result.Exception.Should().BeEquivalentTo(expectedHealthCheckResult.Exception);
            result.Status.Should().Be(expectedHealthCheckResult.Status);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSuppliersAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSupplierByIdAsync(randomSupplier.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllIngestionTrackingsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
