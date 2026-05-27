// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.OptOuts.StuckOptOuts
{
    public partial class OptOutsStuckHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOptOutHealthItemService optOutsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "stuckOptOuts";
        private const string CheckNameDescription = "Stuck Opt Outs";
        private const string ConfigSectionName = "HealthChecks:OptOuts:StuckOptOuts";
        private const int ExpiredAfterDays = 7;
        private const int StuckAfterDays = 1;

        public OptOutsStuckHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string>
            {
                { $"{ConfigSectionName}:DegradedThreshold", "1440" },
                { $"{ConfigSectionName}:UnHealthyThreshold", "2880" },
                { $"{ConfigSectionName}:ExpiredAfterDays", $"{ExpiredAfterDays}" },
                { $"{ConfigSectionName}:StuckAfterDays", $"{StuckAfterDays}" },
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.optOutsHealthItemService = new OptOutsStuckHealthCheckService(
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

        private static List<OptOut> CreateRandomOptOuts(
            DateTimeOffset dateTimeOffset,
            int count,
            bool isExpired = false,
            bool isRecentlySent = false)
        {
            return Enumerable.Range(0, count)
                .Select(_ => CreateRandomOptOut(dateTimeOffset, isExpired, isRecentlySent))
                    .ToList();
        }

        private static OptOut CreateRandomOptOut(
            DateTimeOffset dateTimeOffset,
            bool isExpired = false,
            bool isRecentlySent = false)
        {
            return CreateOptOutFiller(dateTimeOffset, isExpired, isRecentlySent).Create();
        }

        private static Filler<OptOut> CreateOptOutFiller(
            DateTimeOffset dateTimeOffset,
            bool isExpired = false,
            bool isRecentlySent = false)
        {
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

            DateTimeOffset cacheTime = isExpired
                ? currentDateTimeOffset.AddDays(-1 * ExpiredAfterDays).AddMinutes(-1)
                : currentDateTimeOffset.AddDays(1);

            DateTimeOffset lastSentToMesh = isRecentlySent
                ? currentDateTimeOffset
                : currentDateTimeOffset.AddDays(-1 * StuckAfterDays).AddMinutes(-1);

            string user = Guid.NewGuid().ToString();
            var filler = new Filler<OptOut>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(optOut => optOut.CacheTime).Use(cacheTime)
                .OnProperty(optOut => optOut.LastSentToMesh).Use(lastSentToMesh)
                .OnProperty(optOut => optOut.UpdatedDate).Use(dateTimeOffset)
                .OnProperty(optOut => optOut.CreatedBy).Use(user)
                .OnProperty(optOut => optOut.UpdatedBy).Use(user);

            return filler;
        }
    }
}
