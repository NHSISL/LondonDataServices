// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking
{
    public class IngestionTrackingIncompleteBatchHealthCheckService : IIngestionTrackingHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "incompleteBatchesQueue";
        private const string CheckDescriptionName = "Incomplete Batches";
        private const string ConfigSectionName = "HealthChecks:IngestionTracking:IncompleteBatches";

        public IngestionTrackingIncompleteBatchHealthCheckService(
            IStorageBroker storageBroker,
            IConfiguration configuration,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.configuration = configuration;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<HealthCheckResult> GetHealthStatusAsync()
        {
            int degradedThresholdMinutes = configuration
                .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = configuration
                .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
            DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
            var ingestionTrackingQuery = await storageBroker.SelectAllIngestionTrackingsAsync();
            var filteredQuery = ingestionTrackingQuery.Where(ingestionTracking => !ingestionTracking.IsBatchComplete);

            var unHealthyBatches = filteredQuery
                .Where(ingestionTracking => ingestionTracking.UpdatedDate <= unHealthyThresholdDateTime)
                .Select(ingestionTracking => ingestionTracking.Batch)
                .Distinct()
                .ToList();

            var degradedBatches = filteredQuery
                .Where(ingestionTracking =>
                    ingestionTracking.UpdatedDate <= degradedThresholdDateTime &&
                    ingestionTracking.UpdatedDate > unHealthyThresholdDateTime)

                .Select(ingestionTracking => ingestionTracking.Batch)
                .Distinct()
                .Where(batch => !unHealthyBatches.Contains(batch))
                .ToList();

            int unHealthyCount = unHealthyBatches.Count;
            int degradedCount = degradedBatches.Count;
            int totalCount = unHealthyCount + degradedCount;

            string message = totalCount == 0
                ? $"No incomplete batches. All up to date."
                : $"{totalCount} batches incomplete. Please check logs and source locations.";

            var values = new Dictionary<string, object>
            {
                { "description", CheckDescriptionName },
                { "incompleteBatches", totalCount },
                { "degradedItems", degradedCount},
                { "unHealthyItems", unHealthyCount},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message }
            };

            if (unHealthyCount > 0)
            {
                values.Add("status", HealthStatus.Unhealthy.ToString());

                return HealthCheckResult.Unhealthy(
                    description: CheckName,
                    data: values);
            }
            else if (degradedCount > 0)
            {
                values.Add("status", HealthStatus.Degraded.ToString());

                return HealthCheckResult.Degraded(
                    description: CheckName,
                    data: values);
            }
            else
            {
                values.Add("status", HealthStatus.Healthy.ToString());

                return HealthCheckResult.Healthy(
                    description: CheckName,
                    data: values);
            }
        }
    }
}
