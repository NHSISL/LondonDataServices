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

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTrackings.IngestionTrackingDecryptionHealthCheck
{
    public partial class IngestionTrackingDecryptionHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IngestionTrackingDecryptionHealthCheckService ingestionTrackingDecryptionHealthCheckService;
        private const string CheckName = "decryption";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;

        public IngestionTrackingDecryptionHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ingestionTrackingDecryptionHealthCheckService = new IngestionTrackingDecryptionHealthCheckService(
                storageBroker: this.storageBrokerMock.Object,
                configuration: this.configuration,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<IngestionTracking> CreateRandomUnhealthyIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddDays((UnHealthyThresholdMinutes + 1) * -1);
            return CreateIngestionTrackingFiller(unhealthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomDegradedIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);
            return CreateIngestionTrackingFiller(degradedDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomHealthyIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;
            return CreateIngestionTrackingFiller(healthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

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
                .OnProperty(ingestionTracking => ingestionTracking.Decrypted).Use(false)
                .OnProperty(ingestionTracking => ingestionTracking.FileDeleted).Use(false)
                .OnProperty(ingestionTracking => ingestionTracking.IsProcessing).Use(false)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int degradedItemsCount = 0,
            int unhealthyItemsCount = 0)
        {
            var totalItemsCount = degradedItemsCount + unhealthyItemsCount;
            var message = "Nothing to decrypt. All up to date.";

            if (totalItemsCount > 0)
            {
                message = $"{totalItemsCount} files have not been decrypted. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", "Decryption Queue" },
                { "unDecryptedItems", totalItemsCount},
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
