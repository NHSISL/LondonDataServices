using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.HealthChecks.ResolvedAddresses;
using LHDS.Core.Services.Foundations.Audits;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress
{
    public class ResolvedAddressMatchingProcessHealthCheckService : IResolvedAddressHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "addressMatchingTime";
        private const string CheckDescriptionName = "Address Matching Time";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:AddressMatchingTime";

        public ResolvedAddressMatchingProcessHealthCheckService(
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
            int degradedThresholdMinutes = configuration.GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);
            int unHealthyThresholdMinutes = configuration.GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);
            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
            DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
            var auditsQuery = await storageBroker.SelectAllAuditsAsync();

            var filteredQuery = auditsQuery
                .GroupBy(a => a.CorrelationId)
                .Where(g => g.Count() == 1 && g.SingleOrDefault().AuditType == "Resolved Address Match")
                .Select(g => 
                    new ResolvedAddressMatchGroup
                    {
                        CorrelationId = g.Key,
                        ProcessStartDateTime = g.SingleOrDefault().CreatedDate
                    })
                .ToList();

            int degradedCount = filteredQuery.Count(resolvedAddressMatchGroup =>
                resolvedAddressMatchGroup.ProcessStartDateTime <= degradedThresholdDateTime &&
                resolvedAddressMatchGroup.ProcessStartDateTime > unHealthyThresholdDateTime);

            int unHealthyCount = filteredQuery
                .Count(resolvedAddressMatchGroup => 
                    resolvedAddressMatchGroup.ProcessStartDateTime <= unHealthyThresholdDateTime);

            int totalCount = degradedCount + unHealthyCount;

            string message = totalCount == 0
                ? "All processing in acceptable time. All up to date."
                : $"{totalCount} processes running for unacceptable amount of time. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckDescriptionName },
                { "unacceptableProcessingTime", totalCount },
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
