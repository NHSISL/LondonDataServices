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
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking
{
    public partial class IngestionTrackingFilesReceivedHealthCheckService : IIngestionTrackingHealthItemService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IConfiguration configuration;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private const string CheckName = "filesReceived";
        private const string CheckDescriptionName = "Files Received";
        private const string ConfigSectionName = "HealthChecks:IngestionTracking:FilesReceived";

        public IngestionTrackingFilesReceivedHealthCheckService(
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
                var supplierQuery = await storageBroker.SelectAllSuppliersAsync();

                List<Guid> supplierIds = supplierQuery
                    .Where(i => i.IsIngestionTracked == true)
                    .Select(supplier => supplier.Id)
                    .ToList();

                var dataDictionary = new Dictionary<string, object>
                {
                    { "description", CheckDescriptionName },
                    { "checkedAt", currentDateTime.ToString("o") },
                };

                List<HealthCheckResult> results = new List<HealthCheckResult>();

                foreach (Guid supplierId in supplierIds)
                {
                    Supplier supplier = await storageBroker.SelectSupplierByIdAsync(supplierId);
                    var ingestionTrackingQuery = await storageBroker.SelectAllIngestionTrackingsAsync();

                    var filteredQuery = ingestionTrackingQuery
                        .Where(ingestionTracking => ingestionTracking.SupplierId == supplierId
                            && ingestionTracking.CreatedDate <= degradedThresholdDateTime).ToList();

                    int itemsReceived = filteredQuery.Count();

                    bool isDegraded = !filteredQuery.Any(ingestionTracking =>
                        ingestionTracking.CreatedDate > degradedThresholdDateTime);

                    bool isUnhealthy = !filteredQuery.Any(ingestionTracking =>
                        ingestionTracking.CreatedDate <= degradedThresholdDateTime &&
                        ingestionTracking.CreatedDate > unHealthyThresholdDateTime);

                    bool noNonHealthyItems = itemsReceived == 0;

                    HealthStatus status = (noNonHealthyItems, isUnhealthy, isDegraded) switch
                    {
                        (true, _, _) => HealthStatus.Healthy,
                        (_, true, _) => HealthStatus.Unhealthy,
                        (_, _, true) => HealthStatus.Degraded,
                        (_, _, _) => HealthStatus.Healthy
                    };

                    var values = new Dictionary<string, object>
                    {
                        { "description", $"{supplier.Name}" },
                        { "filesReceived", itemsReceived },
                        { "degradedThresholdMinutes", degradedThresholdMinutes.ToString() },
                        { "unHealthyThresholdMinutes", unHealthyThresholdMinutes.ToString() },
                        { "checkedAt", currentDateTime.ToString("o") },
                        { "status", status.ToString() }
                    };

                    HealthCheckResult result = status switch
                    {
                        HealthStatus.Unhealthy => HealthCheckResult.Unhealthy(supplier.Name, data: values),
                        HealthStatus.Degraded => HealthCheckResult.Degraded(supplier.Name, data: values),
                        _ => HealthCheckResult.Healthy(supplier.Name, data: values)
                    };

                    dataDictionary[GetUniqueKey(dataDictionary, result.Description)] = result.Data;
                    results.Add(result);
                }

                int totalFilesReceived = results
                    .Select(result => result.Data.TryGetValue("filesReceived", out var val)
                        && int.TryParse(val?.ToString(), out int count) ? count : 0).Sum();

                dataDictionary.Add("filesReceived", totalFilesReceived);

                if (results.Any(item => item.Status == HealthStatus.Unhealthy))
                {
                    dataDictionary.Add("status", HealthStatus.Unhealthy.ToString());

                    return HealthCheckResult.Unhealthy(
                        description: $"{CheckName}",
                        data: dataDictionary);
                }
                else if (results.Any(item => item.Status == HealthStatus.Degraded))
                {
                    dataDictionary.Add("status", HealthStatus.Degraded.ToString());

                    return HealthCheckResult.Degraded(
                        description: $"{CheckName}",
                        data: dataDictionary);
                }
                else
                {
                    dataDictionary.Add("status", HealthStatus.Healthy.ToString());

                    return HealthCheckResult.Healthy(
                        description: $"{CheckName}",
                        data: dataDictionary);
                }
            });

        private string GetUniqueKey(Dictionary<string, object> dictionary, string baseKey)
        {
            int suffix = 1;
            string key = baseKey;

            while (dictionary.ContainsKey(key))
            {
                key = $"{baseKey} ({suffix++})";
            }

            return key;
        }
    }
}
