// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Services.Foundations.AddressToUprnFileLogs;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityAuditBroker> securityAuditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressToUprnFileLogService addressToUprnFileLogService;

        public AddressToUprnFileLogServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityAuditBrokerMock = new Mock<ISecurityAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressToUprnFileLogService = new AddressToUprnFileLogService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityAuditBroker: this.securityAuditBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

        private static AddressToUprnFileLog CreateRandomModifyAddressToUprnFileLog(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            AddressToUprnFileLog randomAddressToUprnFileLog = CreateRandomAddressToUprnFileLog(dateTimeOffset);

            randomAddressToUprnFileLog.CreatedDate =
                randomAddressToUprnFileLog.CreatedDate.AddDays(randomDaysInPast);

            return randomAddressToUprnFileLog;
        }

        private static AddressToUprnFileLog CreateRandomModifyAddressToUprnFileLog(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            AddressToUprnFileLog randomAddressToUprnFileLog = CreateRandomAddressToUprnFileLog(dateTimeOffset, userId);

            randomAddressToUprnFileLog.CreatedDate =
                randomAddressToUprnFileLog.CreatedDate.AddDays(randomDaysInPast);

            return randomAddressToUprnFileLog;
        }

        private static List<AddressToUprnFileLog> CreateRandomAddressToUprnFileLogs() =>
            CreateAddressToUprnFileLogFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();

        private static AddressToUprnFileLog CreateRandomAddressToUprnFileLog() =>
            CreateAddressToUprnFileLogFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static AddressToUprnFileLog CreateRandomAddressToUprnFileLog(DateTimeOffset dateTimeOffset) =>
            CreateAddressToUprnFileLogFiller(dateTimeOffset).Create();

        private static AddressToUprnFileLog CreateRandomAddressToUprnFileLog(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateAddressToUprnFileLogFiller(dateTimeOffset, userId).Create();

        private static Filler<AddressToUprnFileLog> CreateAddressToUprnFileLogFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AddressToUprnFileLog>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<TimeSpan>().Use(TimeSpan.Zero)
                .OnProperty(log => log.CreatedBy).Use(user)
                .OnProperty(log => log.UpdatedBy).Use(user);

            return filler;
        }

        private static Filler<AddressToUprnFileLog> CreateAddressToUprnFileLogFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<AddressToUprnFileLog>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<TimeSpan>().Use(TimeSpan.Zero)
                .OnProperty(log => log.CreatedBy).Use(userId)
                .OnProperty(log => log.UpdatedBy).Use(userId);

            return filler;
        }
    }
}
