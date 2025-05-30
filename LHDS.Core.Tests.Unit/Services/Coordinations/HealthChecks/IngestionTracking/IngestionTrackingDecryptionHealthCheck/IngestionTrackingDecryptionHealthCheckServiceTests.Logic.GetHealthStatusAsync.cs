using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.IngestionTrackingDecryptionHealthCheck
{
    public partial class IngestionTrackingDecryptionHealthCheckServiceTests
    {
        [Fact]
        public async Task ShouldGetHealthStatusAsyncUnhealthyWhenAnyUnhealthy()
        {
            // given
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> randomTrackings = CreateRandomUnhealthyIngestionTrackings();
            
            var healthCheckResultValues = new Dictionary<string, object>
            {
                { "description", "Decryption Queue" },
                { "unDecryptedItems", randomTrackings.Count()},
                { "degradedItems", 0},
                { "unHealthyItems", randomTrackings.Count()},
                { "degradedThresholdMinutes", 1440 },
                { "unHealthyThresholdMinutes", 2880 },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", $"{randomTrackings.Count()} files have not been decrypted. Please check logs and function status." },
                { "status", HealthStatus.Unhealthy.ToString() }
            };

            var expectedHealthCheckResult = HealthCheckResult.Unhealthy(description: CheckName, data: healthCheckResultValues);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllIngestionTrackingsAsync())
                    .ReturnsAsync(randomTrackings);

            // when
            var result = await this.ingestionTrackingDecryptionHealthCheckService.GetHealthStatusAsync();

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
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
