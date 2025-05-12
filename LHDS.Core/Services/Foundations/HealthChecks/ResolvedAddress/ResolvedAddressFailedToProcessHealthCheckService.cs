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

namespace LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress
{
    public class ResolvedAddressFailedToProcessHealthCheckService : IResolvedAddressHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "failedToProcess";

        public ResolvedAddressFailedToProcessHealthCheckService(
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
            int retryCount = configuration
                .GetValue("HealthChecks:ResolvedAddress:FailedToProcess:RetryCount", 3);

            int degradedThresholdMinutes = configuration
                .GetValue("HealthChecks:ResolvedAddress:FailedToProcess:DegradedThreshold", 1440);

            int unHealthyThresholdMinutes = configuration
                .GetValue("HealthChecks:ResolvedAddress:FailedToProcess:UnHealthyThreshold", 2880);

            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
            DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
            var ingestionTrackingQuery = await storageBroker.SelectAllResolvedAddressesAsync();
            var filteredQuery = ingestionTrackingQuery.Where(i => i.RetryCount >= retryCount);

            int baseCount = filteredQuery.Count(ingestionTracking =>
                ingestionTracking.UpdatedDate > degradedThresholdDateTime);

            int degradedCount = filteredQuery.Count(ingestionTracking =>
                ingestionTracking.UpdatedDate <= degradedThresholdDateTime &&
                ingestionTracking.UpdatedDate > unHealthyThresholdDateTime);

            int unHealthyCount = filteredQuery
                .Count(ingestionTracking => ingestionTracking.UpdatedDate <= unHealthyThresholdDateTime);

            int totalCount = baseCount + degradedCount + unHealthyCount;

            string message = totalCount == 0
                ? $"Nothing to process. All up to date."
                : $"{totalCount} files have not been processed. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", totalCount },
                { "degradedItems", degradedCount},
                { "unHealthyItems", unHealthyCount},
                { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message }
            };

            if (unHealthyCount > 0)
            {
                vals.Add("status", HealthStatus.Unhealthy.ToString());

                return HealthCheckResult.Unhealthy(
                    description: CheckName,
                    data: vals);
            }
            else if (degradedCount > 0)
            {
                vals.Add("status", HealthStatus.Degraded.ToString());

                return HealthCheckResult.Degraded(
                    description: CheckName,
                    data: vals);
            }
            else
            {
                vals.Add("status", HealthStatus.Healthy.ToString());

                return HealthCheckResult.Healthy(
                    description: CheckName,
                    data: vals);
            }
        }
    }
}
