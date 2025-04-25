// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Storages.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.Checks.IngestionTracking.HealthItems
{
    public class IngestionTrackingDecryptionHealthCheckService : IIngestionTrackingHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private const string CheckName = "decryption";

        public IngestionTrackingDecryptionHealthCheckService(
            IStorageBroker storageBroker,
            IConfiguration configuration,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.configuration = configuration;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<HealthCheckResult> GetHealthStatusAsync()
        {
            DateTimeOffset currentDateTime = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            int thresholdMinutes = configuration
                .GetValue("HealthChecks:IngestionTracking:DecryptionThresholdMinutes", 1440);

            DateTimeOffset thresholdDateTime = currentDateTime.AddMinutes(-1 * thresholdMinutes);
            var ingestionTrackingQuery = await storageBroker.SelectAllIngestionTrackingsAsync();

            int decryptionSlowOrStuckCount = ingestionTrackingQuery?
                .Count(ingestionTracking =>
                    ingestionTracking.Decrypted == false &&
                    ingestionTracking.IsProcessing == false &&
                    ingestionTracking.UpdatedDate <= thresholdDateTime) ?? 0;

            string message = decryptionSlowOrStuckCount == 0
                ? $"Nothing to decrypt. All up to date."
                : $"{decryptionSlowOrStuckCount} files have not been decrypted " +
                        $"within the {thresholdMinutes} minute threshold. Please " +
                            $"check that the function is running and check logs for any issues.";

            var vals = new Dictionary<string, object>
            {
                { "description", "Decryption Queue" },
                { "decryptionSlowOrStuckCount", decryptionSlowOrStuckCount },
                { "thresholdMinutes", thresholdMinutes.ToString() },
                { "checkedAt", currentDateTime.ToString("o") },
                { "message", message }
            };

            if (decryptionSlowOrStuckCount > 0)
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
