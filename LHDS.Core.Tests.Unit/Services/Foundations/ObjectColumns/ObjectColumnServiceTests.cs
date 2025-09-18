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
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Services.Foundations.ObjectColumns;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityBroker> securityBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IObjectColumnService objectColumnService;

        public ObjectColumnServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityBrokerMock = new Mock<ISecurityBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.objectColumnService = new ObjectColumnService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityBroker: this.securityBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

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

        private static ObjectColumn CreateRandomModifyObjectColumn(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(dateTimeOffset);

            randomObjectColumn.CreatedDate =
                randomObjectColumn.CreatedDate.AddDays(randomDaysInPast);

            return randomObjectColumn;
        }

        private static ObjectColumn CreateRandomModifyObjectColumn(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(dateTimeOffset, userId);

            randomObjectColumn.CreatedDate =
                randomObjectColumn.CreatedDate.AddDays(randomDaysInPast);

            return randomObjectColumn;
        }

        private static IQueryable<ObjectColumn> CreateRandomObjectColumns()
        {
            return CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static ObjectColumn CreateRandomObjectColumn() =>
            CreateObjectColumnFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ObjectColumn CreateRandomObjectColumn(DateTimeOffset dateTimeOffset) =>
            CreateObjectColumnFiller(dateTimeOffset).Create();

        private static ObjectColumn CreateRandomObjectColumn(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateObjectColumnFiller(dateTimeOffset, userId).Create();

        private static Filler<ObjectColumn> CreateObjectColumnFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(objectColumn => objectColumn.CreatedBy).Use(user)
                .OnProperty(objectColumn => objectColumn.UpdatedBy).Use(user);

            return filler;
        }

        private static Filler<ObjectColumn> CreateObjectColumnFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<ObjectColumn>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(objectColumn => objectColumn.CreatedBy).Use(userId)
                .OnProperty(objectColumn => objectColumn.UpdatedBy).Use(userId);

            return filler;
        }

        private EntraUser CreateRandomEntraUser(string entraUserId = "")
        {
            var userId = string.IsNullOrWhiteSpace(entraUserId) ? GetRandomStringWithLengthOf(255) : entraUserId;

            return new EntraUser(
                entraUserId: userId,
                givenName: GetRandomString(),
                surname: GetRandomString(),
                displayName: GetRandomString(),
                email: GetRandomString(),
                jobTitle: GetRandomString(),
                roles: new List<string> { GetRandomString() },

                claims: new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(type: GetRandomString(), value: GetRandomString())
                },

                authenticationType: "Custom");
        }
    }
}