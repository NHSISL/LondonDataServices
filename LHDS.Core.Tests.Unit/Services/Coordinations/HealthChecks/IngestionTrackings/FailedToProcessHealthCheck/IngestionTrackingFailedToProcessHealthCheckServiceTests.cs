using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTrackings.FailedToProcessHealthCheck
{
    public partial class IngestionTrackingFailedToProcessHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;

        private readonly IngestionTrackingFailedToProcessHealthCheckService 
            ingestionTrackingFailedToProcessHealthCheckService;

        private const string CheckName = "failedToProcess";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;

        public IngestionTrackingFailedToProcessHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ingestionTrackingFailedToProcessHealthCheckService = 
                new IngestionTrackingFailedToProcessHealthCheckService(
                    storageBroker: this.storageBrokerMock.Object,
                    configuration: this.configuration,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object
                );
        }

        private static IQueryable<IngestionTracking> CreateRandomUnhealthyIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);
            return CreateIngestionTrackingFiller(unhealthyDateTime).Create(count: 1).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomDegradedIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);
            return CreateIngestionTrackingFiller(degradedDateTime).Create(count: 1).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomHealthyIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;
            return CreateIngestionTrackingFiller(healthyDateTime).Create(count: 1).AsQueryable();
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset updatedDate
        )
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(updatedDate)
                .OnProperty(ingestionTracking => ingestionTracking.RetryCount).Use(3)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int healthyItemsCount = 0,
            int degradedItemsCount = 0,
            int unhealthyItemsCount = 0)
        {
            var totalItemsCount = healthyItemsCount + degradedItemsCount + unhealthyItemsCount;
            var message = "Nothing to process. All up to date.";

            if (totalItemsCount > 0)
            {
                message = $"{totalItemsCount} files have not been processed. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", "Failed To Process" },
                { "failedToProcess", totalItemsCount},
                { "degradedItems", degradedItemsCount},
                { "unHealthyItems", unhealthyItemsCount},
                { "degradedThresholdMinutes", DegradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", UnHealthyThresholdMinutes.ToString() },
                { "checkedAt", dateTime.ToString("o") },
                { "message", message },
                { "status", healthStatus.ToString() }
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}
