// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Orchestrations.HealthChecks.IngestionTrackings
{
    public class IngestionTrackingHealthCheckOrchestrationService : IHealthCheck
    {
        private readonly IEnumerable<IIngestionTrackingHealthItemService> healthItems;
        private readonly IDateTimeBroker dateTimeBroker;
        private const string healthCheckName = "Ingestion Tracking";

        public IngestionTrackingHealthCheckOrchestrationService(
            IEnumerable<IIngestionTrackingHealthItemService> healthItems,
            IDateTimeBroker dateTimeBroker,
            IConfiguration configuration)
        {
            this.healthItems = healthItems;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var healthTasks = healthItems.Select(service => service.GetHealthStatusAsync().AsTask());
            HealthCheckResult[] results = await Task.WhenAll(healthTasks);

            var dataDictionary = new Dictionary<string, object>
            {
                { "checkedAt", currentDateTime.ToString("o") },
            };

            foreach (HealthCheckResult result in results)
            {
                dataDictionary[GetUniqueKey(dataDictionary, result.Description)] = result.Data;
            }

            if (results.Any(item => item.Status == HealthStatus.Unhealthy))
            {
                return HealthCheckResult.Unhealthy(
                    description: $"{healthCheckName}",
                    data: dataDictionary);
            }
            else if (results.Any(item => item.Status == HealthStatus.Degraded))
            {
                return HealthCheckResult.Degraded(
                    description: $"{healthCheckName}",
                    data: dataDictionary);
            }
            else
            {
                return HealthCheckResult.Healthy(
                    description: $"{healthCheckName}",
                    data: dataDictionary);
            }
        }

        private string GetUniqueKey(Dictionary<string, object> dictionary, string baseKey)
        {
            int suffix = 1;
            string key = baseKey;

            while (dictionary.ContainsKey(key))
            {
                key = $"{baseKey} ({suffix++})";
            }

            return key;
        }
    }
}
