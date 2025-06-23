using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyPolls.FailedToProcess
{
    public partial class TerminologyPollsFailedToProcessHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyPollsHealthItemService terminologyPollsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "failedToProcess";
        private const string CheckNameDescription = "Failed To Process";
        private const string ConfigSectionName = "HealthChecks:TerminologyPolls:FailedToProcess";

        public TerminologyPollsFailedToProcessHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string> {
                {$"{ConfigSectionName}:DegradedThreshold", "1440"},
                {$"{ConfigSectionName}:UnHealthyThreshold", "2880"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.terminologyPollsHealthItemService = new TerminologyPollsFailedToProcessHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<HealthCheckResult, bool>> SameHealthCheckResultAs(
            HealthCheckResult expectedHealthCheckResult) =>
                actualHealthCheckResult => compareLogic.Compare(expectedHealthCheckResult, actualHealthCheckResult)
                    .AreEqual;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<TerminologyPoll> CreateRandomTerminologyPolls(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateTerminologyPollsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyPoll> CreateTerminologyPollsFiller(
            DateTimeOffset lastPollDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(terminologyPolls => terminologyPolls.LastPoll).Use(lastPollDateTime)
                .OnProperty(terminologyPolls => terminologyPolls.ResourceType).Use(resourceType)
                .OnProperty(terminologyPolls => terminologyPolls.CreatedBy).Use(user)
                .OnProperty(terminologyPolls => terminologyPolls.UpdatedBy).Use(user);

            return filler;
        }
    }
}
