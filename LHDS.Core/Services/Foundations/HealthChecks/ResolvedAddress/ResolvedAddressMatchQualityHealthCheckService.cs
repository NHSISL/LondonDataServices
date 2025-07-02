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
    public partial class ResolvedAddressMatchQualityHealthCheckService : IResolvedAddressHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "matchQuality";
        private const string CheckDescriptionName = "Match Quality";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:MatchQuality";

        public ResolvedAddressMatchQualityHealthCheckService(
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
                double degradedThresholdPercentage = configuration.GetValue($"{ConfigSectionName}:DegradedThresholdPercentage", 0.9);
                double unHealthyThresholdPercentage = configuration.GetValue($"{ConfigSectionName}:UnHealthyThresholdPercentage", 0.8);
                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                DateTimeOffset queryCutOffDate = currentDateTime.AddDays(-1);
                var resolvedAddressQuery = await storageBroker.SelectAllResolvedAddressesAsync();
                var filteredQuery = resolvedAddressQuery.Where(i => i.UpdatedDate >= queryCutOffDate);
                int totalCount = filteredQuery.Count();
                int matchedCount = filteredQuery.Where(i => i.MatchedWithAssign == true).Count();
                double percentageMatched = (double) matchedCount / (double) totalCount;

                bool isDegraded = (percentageMatched > unHealthyThresholdPercentage
                    && percentageMatched <= degradedThresholdPercentage);

                bool isUnHealthy = percentageMatched <= unHealthyThresholdPercentage;

                string message = (!isDegraded && !isUnHealthy)
                    ? "Match quality is good"
                    : $"{percentageMatched * 100}% average match rate. Please check logs and function status.";

                var vals = new Dictionary<string, object>
                {
                    { "description", CheckDescriptionName },
                    { "averageMatchRate", percentageMatched },
                    { "isDegraded", isDegraded},
                    { "isUnhealthy", isUnHealthy},
                    { "degradedThresholdPercentage", degradedThresholdPercentage.ToString() },
                    { "unHealthyThresholdPercentage", unHealthyThresholdPercentage.ToString() },
                    { "checkedAt", currentDateTime.ToString("o") },
                    { "message", message }
                };

                if (isUnHealthy)
                {
                    vals.Add("status", HealthStatus.Unhealthy.ToString());

                    return HealthCheckResult.Unhealthy(
                        description: CheckName,
                        data: vals);
                }
                else if (isDegraded)
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
