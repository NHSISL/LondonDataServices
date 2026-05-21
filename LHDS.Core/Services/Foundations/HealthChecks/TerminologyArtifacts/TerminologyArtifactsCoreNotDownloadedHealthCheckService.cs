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

namespace LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts
{
    public partial class TerminologyArtifactsCoreNotDownloadedHealthCheckService : ITerminologyArtifactsHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "coreNotDownloaded";
        private const string CheckNameDescription = "Core Not Downloaded";

        public TerminologyArtifactsCoreNotDownloadedHealthCheckService(
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
                DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                var terminologyArtifactsQuery = await storageBroker.SelectAllTerminologyArtifactsAsync();

                var filteredQuery = terminologyArtifactsQuery
                    .Where(terminologyArtifact =>
                        terminologyArtifact.IsCore
                        && !terminologyArtifact.IsDownloaded
                        && !terminologyArtifact.IsError);

                int codeSystemCount = filteredQuery
                    .Count(terminologyArtifact => terminologyArtifact.ResourceType == "CodeSystem");

                int conceptMapCount = filteredQuery
                    .Count(terminologyArtifact => terminologyArtifact.ResourceType == "ConceptMap");

                int valueSetCount = filteredQuery
                    .Count(terminologyArtifact => terminologyArtifact.ResourceType == "ValueSet");

                int totalCount = codeSystemCount + conceptMapCount + valueSetCount;

                string message = totalCount == 0
                    ? "All core terminology artifacts have been downloaded."
                    : $"{totalCount} core terminology artifacts have not been downloaded. " +
                        $"Please check logs and function status.";

                var vals = new Dictionary<string, object>
                {
                    { "description", CheckNameDescription },
                    { "coreNotDownloaded", totalCount },
                    { "codeSystemItems", codeSystemCount },
                    { "conceptMapItems", conceptMapCount },
                    { "valueSetItems", valueSetCount },
                    { "checkedAt", currentDateTime.ToString("o") },
                    { "message", message }
                };

                if (totalCount > 0)
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
            });
    }
}
