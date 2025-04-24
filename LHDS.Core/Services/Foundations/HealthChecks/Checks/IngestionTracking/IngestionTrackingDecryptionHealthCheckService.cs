// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.Checks.IngestionTracking
{
    public class IngestionTrackingDecryptionHealthCheckService : IHealthCheck
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;

        public IngestionTrackingDecryptionHealthCheckService(
            IStorageBroker storageBroker,
            IConfiguration configuration,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.configuration = configuration;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            int thresholdMinutes = this.configuration.GetValue<int>("IngestionTracking:ThresholdMinutes", 1440);
            DateTimeOffset thresholdDateTime = currentDateTime.AddMinutes(-1 * thresholdMinutes);
            var ingestionTrackingQuery = await this.storageBroker.SelectAllIngestionTrackingsAsync();

            var decryptionSlowOrStuckCount = ingestionTrackingQuery
                .Count(ingestionTracking => ingestionTracking.Decrypted == false
                    && ingestionTracking.IsProcessing == false
                        && ingestionTracking.UpdatedDate <= thresholdDateTime);

            var vals = new Dictionary<string, object>
            {
                { "QueueLength", decryptionSlowOrStuckCount },
                { "ThresholdMinutes", thresholdMinutes },

                { "Message",
                  $"{decryptionSlowOrStuckCount} files have not be decrypted within " +
                    $"the {thresholdMinutes} minute threshold.  Please check that the function " +
                        $"is running and logs for any issues."
                },
            };

            if (decryptionSlowOrStuckCount > 0 || stuckInProcessingCount > 0)
            {
                return HealthCheckResult.Degraded(
                    description: "Ingestion Tracking - Decryption is slow or stuck.",
                    data: vals);
            }
            else
            {
                return HealthCheckResult.Healthy(
                    description: "Ingestion Tracking - Decryption is healthy.",
                    data: vals);
            }
        }
    }
}
