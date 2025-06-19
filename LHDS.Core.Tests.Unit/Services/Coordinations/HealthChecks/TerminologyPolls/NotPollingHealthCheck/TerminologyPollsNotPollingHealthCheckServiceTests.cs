using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Services.Foundations.HealthChecks.TerminologyPolls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyPolls.NotPollingHealthCheck
{
    public partial class TerminologyPollsNotPollingHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;

        private readonly TerminologyPollsNotPollingHealthCheckService
            terminologyPollsNotPollingHealthCheckService;

        private const string CheckName = "terminologyPolls";
        private const string CheckNameDescription = "Terminology Polls";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;

        public TerminologyPollsNotPollingHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.terminologyPollsNotPollingHealthCheckService =
                new TerminologyPollsNotPollingHealthCheckService(
                    storageBroker: this.storageBrokerMock.Object,
                    configuration: this.configuration,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<TerminologyPoll> CreateRandomUnhealthyTerminologyPolls()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);

            return CreateTerminologyPollFiller(unhealthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<TerminologyPoll> CreateRandomDegradedTerminologyPolls()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);

            return CreateTerminologyPollFiller(degradedDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<TerminologyPoll> CreateRandomHealthyTerminologyPolls()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;

            return CreateTerminologyPollFiller(healthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<TerminologyPoll> CreateTerminologyPollFiller(
            DateTimeOffset lastPollDateTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyPoll => terminologyPoll.CreatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.LastPoll).Use(lastPollDateTime);              

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int degradedItemsCount = 0,
            int unhealthyItemsCount = 0)
        {
            var nonHealthyItemsCount = degradedItemsCount + unhealthyItemsCount;
            var message = "Nothing to poll. All up to date.";

            if (nonHealthyItemsCount > 0)
            {
                message = $"{nonHealthyItemsCount} not polling. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "notPolling", nonHealthyItemsCount},
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
