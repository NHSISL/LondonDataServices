using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.PDS
{
    public class PdsReceivedReplyHealthCheckService : IPdsHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "receivedReply";
        private const string CheckNameDescription = "Received Reply";
        private const string ConfigSectionName = "HealthChecks:Pds:ReceivedReply";

        public PdsReceivedReplyHealthCheckService(
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
            int unHealthyThresholdMinutes = configuration
                .GetValue($"${ConfigSectionName}:UnHealthyThreshold", 1440);

            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
            var pdsAuditQuery = await storageBroker.SelectAllPdsAuditsAsync();

            var filteredQuery = pdsAuditQuery.Where(i => i.UpdatedDate <= unHealthyThresholdDateTime
                && i.IsCompleted == false);

            int unHealthyCount = filteredQuery.Count();

            string message = unHealthyCount == 0
                ? $"All requests received reply"
                : $"{unHealthyCount} request have no reply. Please check logs and function status.";

            var vals = new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "notReceivedReply", unHealthyCount },
                { "unHealthyItems", unHealthyCount},
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
