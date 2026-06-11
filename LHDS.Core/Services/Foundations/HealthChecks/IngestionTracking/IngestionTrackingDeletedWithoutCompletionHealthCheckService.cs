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
    public partial class IngestionTrackingDeletedWithoutCompletionHealthCheckService : IIngestionTrackingHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "deletedWithoutCompletion";
        private const string CheckDescriptionName = "Deleted Without Completion";
        private const string ConfigSectionName = "HealthChecks:IngestionTracking:DeletedWithoutCompletion";

        public IngestionTrackingDeletedWithoutCompletionHealthCheckService(
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

        public ValueTask<HealthCheckResult> GetHealthStatusAsync() =>
            TryCatch(async () =>
            {
                int degradedThresholdMinutes = configuration
                    .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

                int unHealthyThresholdMinutes = configuration
                    .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
                DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
                var ingestionTrackingQuery = await storageBroker.SelectAllIngestionTrackingsAsync();

                var filteredQuery = ingestionTrackingQuery.Where(ingestionTracking =>
                    ingestionTracking.FileDeleted && !ingestionTracking.Decrypted);

                int degradedCount = filteredQuery.Count(ingestionTracking =>
                    ingestionTracking.UpdatedDate <= degradedThresholdDateTime &&
                    ingestionTracking.UpdatedDate > unHealthyThresholdDateTime);

                int unHealthyCount = filteredQuery
                    .Count(ingestionTracking => ingestionTracking.UpdatedDate <= unHealthyThresholdDateTime);

                int totalCount = degradedCount + unHealthyCount;

                string message = totalCount == 0
                    ? $"Nothing to report. All files completed before deletion."
                    : $"{totalCount} files were deleted before completing the pipeline. " +
                        $"Please check logs and function status.";

                var values = new Dictionary<string, object>
                {
                    { "description", CheckDescriptionName },
                    { "deletedWithoutCompletionItems", totalCount },
                    { "degradedItems", degradedCount },
                    { "unHealthyItems", unHealthyCount },
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
            });
    }
}
