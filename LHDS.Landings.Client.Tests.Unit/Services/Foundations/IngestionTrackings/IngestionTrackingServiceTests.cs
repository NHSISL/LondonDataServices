// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages.Sql;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Services.Foundations.IngestionTrackings;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Landings.Client.Tests.Unit.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IIngestionTrackingService ingestionTrackingService;

        public IngestionTrackingServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ingestionTrackingService = new IngestionTrackingService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
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
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IngestionTracking CreateRandomModifyIngestionTracking(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(dateTimeOffset);

            randomIngestionTracking.CreatedDate =
                randomIngestionTracking.CreatedDate.AddDays(randomDaysInPast);

            return randomIngestionTracking;
        }

        private static IQueryable<IngestionTracking> CreateRandomIngestionTrackings()
        {
            return CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();
            filler.Setup().OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}