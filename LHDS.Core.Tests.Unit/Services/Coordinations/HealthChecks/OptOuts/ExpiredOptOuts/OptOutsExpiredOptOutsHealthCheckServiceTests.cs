using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.HealthChecks.OptOut;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.OptOuts.ExpiredOptOuts
{
    public partial class OptOutsExpiredOptOutsHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly OptOutsExpiredOptOutHealthCheckService optOutExpiredOptOutHealthCheckService;
        private const string CheckName = "expiredOptOuts";
        private const string CheckNameDescription = "Expired Opt Outs";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;
        private const int ExpiredAfterDays = 7;
        private const int LastSentExpiredAfterDays = 2;

        public OptOutsExpiredOptOutsHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.optOutExpiredOptOutHealthCheckService = new OptOutsExpiredOptOutHealthCheckService(
                storageBroker: this.storageBrokerMock.Object,
                configuration: this.configuration,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<OptOut> CreateRandomUnhealthyOptOuts()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddDays((UnHealthyThresholdMinutes + 1) * -1);
            return CreateOptOutFiller(unhealthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<OptOut> CreateRandomDegradedOptOuts()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);
            return CreateOptOutFiller(degradedDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<OptOut> CreateRandomHealthyOptOuts()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;
            return CreateOptOutFiller(healthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Filler<OptOut> CreateOptOutFiller(
            DateTimeOffset updatedDate
        )
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset expiredDateTimeOffset = dateTimeOffset.AddDays(-1 * ExpiredAfterDays).AddMinutes(-1);
            DateTimeOffset lastSentExpiredDateTimeOffset = dateTimeOffset.AddDays(-1 * LastSentExpiredAfterDays).AddMinutes(-1);
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user)
                .OnProperty(OptOuts => OptOuts.CacheTime).Use(expiredDateTimeOffset)
                .OnProperty(OptOuts => OptOuts.LastSentToMesh).Use(lastSentExpiredDateTimeOffset)
                .OnProperty(optOut => optOut.UpdatedDate).Use(updatedDate);

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int degradedItemsCount = 0,
            int unhealthyItemsCount = 0)
        {
            var totalItemsCount = degradedItemsCount + unhealthyItemsCount;
            var message = "Nothing is expired and outdated. All up to date.";

            if (totalItemsCount > 0)
            {
                message = $"{totalItemsCount} opt outs expired and outdated. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "expiredAndOutdated", totalItemsCount},
                { "degradedItems", degradedItemsCount},
                { "unHealthyItems", unhealthyItemsCount},
                { "degradedThresholdMinutes", DegradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", UnHealthyThresholdMinutes.ToString() },
                { "checkedAt", dateTime.ToString("o") },
                { "message", message },
                { "status", healthStatus.ToString() }
            };
        }
    }
}
