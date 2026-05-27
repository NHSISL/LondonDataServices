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

namespace LHDS.Core.Services.Foundations.HealthChecks.PDS
{
    public partial class PdsAuditPipelineAliveHealthCheckService : IPdsHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "pipelineAlive";
        private const string CheckDescriptionName = "Pipeline Alive";
        private const string ConfigSectionName = "HealthChecks:PdsAudit:PipelineAlive";

        public PdsAuditPipelineAliveHealthCheckService(
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
                var pdsAuditsQuery = await storageBroker.SelectAllPdsAuditsAsync();

                if (!pdsAuditsQuery.Any())
                {
                    var emptyValues = new Dictionary<string, object>
                    {
                        { "description", CheckDescriptionName },
                        { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                        { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                        { "checkedAt", currentDateTime.ToString("o") },
                        { "message", "No PDS audit records exist yet." },
                        { "status", HealthStatus.Healthy.ToString() }
                    };

                    return HealthCheckResult.Healthy(
                        description: CheckName,
                        data: emptyValues);
                }

                bool hasRecentActivity = pdsAuditsQuery
                    .Any(pdsAudit => pdsAudit.CreatedDate >= degradedThresholdDateTime);

                bool hasActivityWithinUnhealthyWindow = pdsAuditsQuery
                    .Any(pdsAudit => pdsAudit.CreatedDate >= unHealthyThresholdDateTime);

                string message;

                var values = new Dictionary<string, object>
                {
                    { "description", CheckDescriptionName },
                    { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                    { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                    { "checkedAt", currentDateTime.ToString("o") },
                };

                if (!hasActivityWithinUnhealthyWindow)
                {
                    message = $"No PDS audit records have been received in the last {unHealthyThresholdMinutes} minutes. " +
                        $"The PDS pipeline may have stopped. Please check logs and function status.";

                    values.Add("message", message);
                    values.Add("status", HealthStatus.Unhealthy.ToString());

                    return HealthCheckResult.Unhealthy(
                        description: CheckName,
                        data: values);
                }
                else if (!hasRecentActivity)
                {
                    message = $"No PDS audit records have been received in the last {degradedThresholdMinutes} minutes. " +
                        $"The PDS pipeline may be slow. Please check logs and function status.";

                    values.Add("message", message);
                    values.Add("status", HealthStatus.Degraded.ToString());

                    return HealthCheckResult.Degraded(
                        description: CheckName,
                        data: values);
                }
                else
                {
                    message = "PDS audit pipeline is alive. Recent activity detected.";
                    values.Add("message", message);
                    values.Add("status", HealthStatus.Healthy.ToString());

                    return HealthCheckResult.Healthy(
                        description: CheckName,
                        data: values);
                }
            });
    }
}
