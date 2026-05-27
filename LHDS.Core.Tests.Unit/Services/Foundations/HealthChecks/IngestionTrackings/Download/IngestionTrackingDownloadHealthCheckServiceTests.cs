// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.Download
{
    public partial class IngestionTrackingDownloadHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IIngestionTrackingHealthItemService ingestionTrackingHealthItemService;
        private readonly ICompareLogic compareLogic;

        public IngestionTrackingDownloadHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string>
            {
                { "HealthChecks:IngestionTracking:Download:DegradedThreshold", "1440" },
                { "HealthChecks:IngestionTracking:Download:UnHealthyThreshold", "2880" },
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.ingestionTrackingHealthItemService = new IngestionTrackingDownloadHealthCheckService(
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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

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

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<IngestionTracking> CreateRandomIngestionTrackings(
            DateTimeOffset dateTimeOffset,
            bool isDownloaded,
            bool isProcessing,
            bool fileDeleted,
            int count)
        {
            return Enumerable.Range(0, count)
                .Select(_ => CreateRandomIngestionTracking(dateTimeOffset, isDownloaded, isProcessing, fileDeleted))
                    .ToList();
        }

        private static IngestionTracking CreateRandomIngestionTracking(
            DateTimeOffset dateTimeOffset,
            bool isDownloaded,
            bool isProcessing,
            bool fileDeleted) =>
                CreateIngestionTrackingFiller(dateTimeOffset, isDownloaded, isProcessing, fileDeleted).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            bool isDownloaded,
            bool isProcessing,
            bool fileDeleted)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.IsDownloaded).Use(isDownloaded)
                .OnProperty(ingestionTracking => ingestionTracking.IsProcessing).Use(isProcessing)
                .OnProperty(ingestionTracking => ingestionTracking.FileDeleted).Use(fileDeleted)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt();

            return filler;
        }
    }
}
