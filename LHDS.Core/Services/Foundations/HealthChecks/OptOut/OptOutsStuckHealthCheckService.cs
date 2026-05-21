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

namespace LHDS.Core.Services.Foundations.HealthChecks.OptOut
{
    public partial class OptOutsStuckHealthCheckService : IOptOutHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "stuckOptOuts";
        private const string CheckNameDescription = "Stuck Opt Outs";
        private const string ConfigSectionName = "HealthChecks:OptOuts:StuckOptOuts";

        public OptOutsStuckHealthCheckService(
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

                int expiredAfterDays = configuration
                    .GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

                int stuckAfterDays = configuration
                    .GetValue($"{ConfigSectionName}:StuckAfterDays", 1);

                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                DateTimeOffset expirationDate = currentDateTime.AddDays(-1 * expiredAfterDays);
                DateTimeOffset stuckThresholdDate = currentDateTime.AddDays(-1 * stuckAfterDays);
                DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
                DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
                var optOutsQuery = await storageBroker.SelectAllOptOutsAsync();

                var filteredQuery = optOutsQuery.Where(optOut =>
                    optOut.CacheTime >= expirationDate
                    && optOut.LastSentToMesh < stuckThresholdDate);

                int degradedCount = filteredQuery.Count(optOut =>
                    optOut.UpdatedDate <= degradedThresholdDateTime &&
                    optOut.UpdatedDate > unHealthyThresholdDateTime);

                int unHealthyCount = filteredQuery
                    .Count(optOut => optOut.UpdatedDate <= unHealthyThresholdDateTime);

                int totalCount = degradedCount + unHealthyCount;

                string message = totalCount == 0
                    ? "Nothing is stuck. All opt outs are being sent to MESH."
                    : $"{totalCount} opt outs are active but have not been sent to MESH recently. " +
                        "Please check logs and function status.";

                var values = new Dictionary<string, object>
                {
                    { "description", CheckNameDescription },
                    { "stuckOptOuts", totalCount },
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
