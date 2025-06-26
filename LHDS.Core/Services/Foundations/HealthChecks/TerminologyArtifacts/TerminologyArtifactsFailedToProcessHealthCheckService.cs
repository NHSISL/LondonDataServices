using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckService : ITerminologyArtifactsHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "failedToProcess";
        private const string CheckNameDescription = "Failed To Process";
        private const string ConfigSectionName = "HealthChecks:TerminologyArtifacts:FailedToProcess";

        public TerminologyArtifactsFailedToProcessHealthCheckService(
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
                {
                    int degradedThresholdMinutes = configuration
                        .GetValue($"{ConfigSectionName}:DegradedThreshold", 1440);

                    int unHealthyThresholdMinutes = configuration
                        .GetValue($"{ConfigSectionName}:UnHealthyThreshold", 2880);

                    DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    DateTimeOffset degradedThresholdDateTime = currentDateTime.AddMinutes(-1 * degradedThresholdMinutes);
                    DateTimeOffset unHealthyThresholdDateTime = currentDateTime.AddMinutes(-1 * unHealthyThresholdMinutes);
                    var terminologyArtifactsQuery = await storageBroker.SelectAllTerminologyArtifactsAsync();

                    var filteredQuery = terminologyArtifactsQuery.Where(terminologyArtifact => terminologyArtifact.IsError);

                    var degradedQuery = terminologyArtifactsQuery.Where(terminologyPoll =>
                        terminologyPoll.UpdatedDate <= degradedThresholdDateTime &&
                        terminologyPoll.UpdatedDate > unHealthyThresholdDateTime);

                    var unHealthyQuery = terminologyArtifactsQuery
                        .Where(terminologyPoll => terminologyPoll.UpdatedDate <= unHealthyThresholdDateTime);

                    int codeSystemDegradedCount = degradedQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "CodeSystem");

                    int codeSystemUnhealthyCount = unHealthyQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "CodeSystem");

                    int conceptMapDegradedCount = degradedQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "ConceptMap");

                    int conceptMapUnhealthyCount = unHealthyQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "ConceptMap");

                    int valueSetDegradedCount = degradedQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "ValueSet");

                    int valueSetMapUnhealthyCount = unHealthyQuery
                        .Count(terminologyPoll => terminologyPoll.ResourceType == "ValueSet");

                    int totalCount = codeSystemDegradedCount
                        + codeSystemUnhealthyCount
                        + conceptMapDegradedCount
                        + conceptMapUnhealthyCount
                        + valueSetDegradedCount
                        + valueSetMapUnhealthyCount;

                    string message = totalCount == 0
                        ? $"Nothing to process. All up to date."
                        : $"{totalCount} have not been processed. Please check logs and function status.";

                    var vals = new Dictionary<string, object>
                    {
                        { "description", CheckNameDescription },
                        { "failedToProcess", totalCount },
                        { "degradedCodeSystemItems", codeSystemDegradedCount},
                        { "unHealthyCodeSystemItems", codeSystemUnhealthyCount},
                        { "degradedConceptMapItems", conceptMapDegradedCount},
                        { "unHealthyConceptMapItems", conceptMapUnhealthyCount},
                        { "degradedValueSetItems", valueSetDegradedCount},
                        { "unHealthyValueItems", valueSetMapUnhealthyCount},
                        { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                        { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                        { "checkedAt", currentDateTime.ToString("o") },
                        { "message", message }
                    };

                    if ((codeSystemUnhealthyCount + conceptMapUnhealthyCount + valueSetMapUnhealthyCount) > 0)
                    {
                        vals.Add("status", HealthStatus.Unhealthy.ToString());

                        return HealthCheckResult.Unhealthy(
                            description: CheckName,
                            data: vals);
                    }
                    else if ((codeSystemDegradedCount + conceptMapDegradedCount + valueSetDegradedCount) > 0)
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
            });
    }
}
