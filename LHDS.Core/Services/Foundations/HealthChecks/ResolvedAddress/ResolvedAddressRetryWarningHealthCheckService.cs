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
    public partial class ResolvedAddressRetryWarningHealthCheckService : IResolvedAddressHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "retryWarning";
        private const string CheckDescriptionName = "Retry Warning";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:RetryWarning";

        public ResolvedAddressRetryWarningHealthCheckService(
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
                int maxRetryCount = configuration
                    .GetValue($"{ConfigSectionName}:MaxRetryCount", 3);

                int degradedThresholdMinutes = configuration
                    .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

                int unHealthyThresholdMinutes = configuration
                    .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
                DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
                var resolvedAddressQuery = await storageBroker.SelectAllResolvedAddressesAsync();

                var filteredQuery = resolvedAddressQuery.Where(resolvedAddress =>
                    resolvedAddress.RetryCount > 0 && resolvedAddress.RetryCount < maxRetryCount);

                int degradedCount = filteredQuery.Count(resolvedAddress =>
                    resolvedAddress.UpdatedDate <= degradedThresholdDateTime &&
                    resolvedAddress.UpdatedDate > unHealthyThresholdDateTime);

                int unHealthyCount = filteredQuery
                    .Count(resolvedAddress => resolvedAddress.UpdatedDate <= unHealthyThresholdDateTime);

                int totalCount = degradedCount + unHealthyCount;

                string message = totalCount == 0
                    ? $"Nothing to warn about. All up to date."
                    : $"{totalCount} resolved addresses have had retries but have not yet failed. " +
                        $"Please check logs and function status.";

                var values = new Dictionary<string, object>
                {
                    { "description", CheckDescriptionName },
                    { "retryWarningItems", totalCount },
                    { "degradedItems", degradedCount },
                    { "unHealthyItems", unHealthyCount },
                    { "maxRetryCount", maxRetryCount.ToString() },
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
