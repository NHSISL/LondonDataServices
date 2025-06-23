using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Services.Foundations.HealthChecks.OptOut;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.ExpiredOptOuts
{
    public partial class OptOutsExpiredOptOutsHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOptOutHealthItemService OptOutsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "expiredOptOuts";
        private const string CheckNameDescription = "Expired Opt Outs";
        private const string ConfigSectionName = "HealthChecks:OptOuts:ExpiredOptOuts";
        private const int ExpiredAfterDays = 7;
        private const int LastSentExpiredAfterDays = 2;

        public OptOutsExpiredOptOutsHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string> {
                {$"{ConfigSectionName}:DegradedThreshold", "1440"},
                {$"{ConfigSectionName}:UnHealthyThreshold", "2880"},
                {$"{ConfigSectionName}:ExpiredAfterDays", "7"},
                {$"{ConfigSectionName}:LastSentExpiredAfterDays", "2"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.OptOutsHealthItemService = new OptOutsExpiredOptOutHealthCheckService(
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

        public static TheoryData<int> MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static List<OptOut> CreateRandomOptOuts(
            DateTimeOffset dateTimeOffset,
            int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => CreateRandomOptOut(dateTimeOffset))
                    .ToList();
        }

        private static OptOut CreateRandomOptOut(DateTimeOffset dateTimeOffset) =>
                CreateOptOutFiller(dateTimeOffset).Create();

        private static Filler<OptOut> CreateOptOutFiller(
            DateTimeOffset dateTimeOffset)
        {
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset expiredDateTimeOffset = currentDateTimeOffset.AddDays(-1 * ExpiredAfterDays).AddMinutes(-1);
            DateTimeOffset lastSentExpiredDateTimeOffset = currentDateTimeOffset.AddDays(-1 * LastSentExpiredAfterDays).AddMinutes(-1);
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(OptOuts => OptOuts.CacheTime).Use(expiredDateTimeOffset)
                .OnProperty(OptOuts => OptOuts.LastSentToMesh).Use(lastSentExpiredDateTimeOffset)
                .OnProperty(OptOuts => OptOuts.UpdatedDate).Use(dateTimeOffset)
                .OnProperty(OptOuts => OptOuts.CreatedBy).Use(user)
                .OnProperty(OptOuts => OptOuts.UpdatedBy).Use(user);

            return filler;
        }
    }
}
