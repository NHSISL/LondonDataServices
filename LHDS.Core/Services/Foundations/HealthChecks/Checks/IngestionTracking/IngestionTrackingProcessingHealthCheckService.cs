// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.Checks.IngestionTracking
{
    public class IngestionTrackingProcessingHealthCheckService : IHealthCheck
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;

        public IngestionTrackingProcessingHealthCheckService(
            IStorageBroker storageBroker,
            IConfiguration configuration,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.configuration = configuration;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            int thresholdMinutes = this.configuration
                .GetValue<int>("HealthChecks:IngestionTracking:ProcessingThresholdMinutes", 1440);

            DateTimeOffset thresholdDateTime = currentDateTime.AddMinutes(-1 * thresholdMinutes);
            var ingestionTrackingQuery = await this.storageBroker.SelectAllIngestionTrackingsAsync();

            var stuckInProcessingCount = ingestionTrackingQuery?
                .Count(ingestionTracking =>
                    ingestionTracking.IsProcessing == true && ingestionTracking.UpdatedDate <= thresholdDateTime) ?? 0;

            string message = stuckInProcessingCount == 0
                ? $"Nothing to process. All up to date."
                : $"{stuckInProcessingCount} files have been stuck in processing for more than the " +
                    $"{thresholdMinutes} minute threshold.  Please see the logs for any issues and " +
                        $"set the status for IsProcessing to FALSE so the queue can re-process them.";

            var vals = new Dictionary<string, object>
            {
                { "stuckInProcessing", stuckInProcessingCount },
                { "thresholdMinutes", thresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message }
            };

            if (stuckInProcessingCount > 0)
            {
                return HealthCheckResult.Degraded(
                    description: "Ingestion Tracking - Processing is slow or stuck.",
                    data: vals);
            }
            else
            {
                return HealthCheckResult.Healthy(
                    description: "Ingestion Tracking - Processing is healthy.",
                    data: vals);
            }
        }
    }
}
