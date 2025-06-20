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
    public partial class OptOutsExpiredOptOutHealthCheckService : IOptOutHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "expiredOptOuts";
        private const string CheckNameDescription = "Expired Opt Outs";
        private const string ConfigSectionName = "HealthChecks:OptOuts:ExpiredOptOuts";

        public OptOutsExpiredOptOutHealthCheckService(
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
                int degradedThresholdMinutes = configuration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

                int unHealthyThresholdMinutes =
                configuration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

                int expiredAfterDays = configuration.GetValue($"{ConfigSectionName}:ExpiredAfterDays", 7);

                int lastSentExpiredAfterDays = configuration
                    .GetValue($"{ConfigSectionName}:LastSentExpiredAfterDays", 2);

                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                DateTimeOffset expirationDate = currentDateTime.AddDays(-1 * expiredAfterDays);
                DateTimeOffset lastSentExpirationDate = currentDateTime.AddDays(-1 * lastSentExpiredAfterDays);
                DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
                DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
                var optOutsQuery = await storageBroker.SelectAllOptOutsAsync();

                var filteredQuery = optOutsQuery.Where(i => i.CacheTime < expirationDate
                    && i.LastSentToMesh < lastSentExpirationDate);

                int degradedCount = filteredQuery.Count(optOut =>
                    optOut.UpdatedDate <= degradedThresholdDateTime &&
                    optOut.UpdatedDate > unHealthyThresholdDateTime);

                int unHealthyCount = filteredQuery
                    .Count(optOut => optOut.UpdatedDate <= unHealthyThresholdDateTime);

                int totalCount = degradedCount + unHealthyCount;

                string message = totalCount == 0
                    ? $"Nothing is expired and outdated. All up to date."
                    : $"{totalCount} opt outs expired and outdated. Please check logs and function status.";

                var vals = new Dictionary<string, object>
                {
                    { "description", CheckNameDescription },
                    { "expiredAndOutdated", totalCount },
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
            });
    }
}
