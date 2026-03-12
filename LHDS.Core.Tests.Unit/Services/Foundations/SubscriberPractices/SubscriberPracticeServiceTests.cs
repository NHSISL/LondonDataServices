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
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Services.Foundations.SubscriberPractices;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityAuditBroker> securityAuditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISubscriberPracticeService subscriberPracticeService;

        public SubscriberPracticeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityAuditBrokerMock = new Mock<ISecurityAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.subscriberPracticeService = new SubscriberPracticeService(
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

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

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

        private static SubscriberPractice CreateRandomModifySubscriberPractice(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice(dateTimeOffset);

            randomSubscriberPractice.CreatedDate =
                randomSubscriberPractice.CreatedDate.AddDays(randomDaysInPast);

            return randomSubscriberPractice;
        }

        private static SubscriberPractice CreateRandomModifySubscriberPractice(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice(dateTimeOffset, userId);

            randomSubscriberPractice.CreatedDate =
                randomSubscriberPractice.CreatedDate.AddDays(randomDaysInPast);

            return randomSubscriberPractice;
        }

        private static IQueryable<SubscriberPractice> CreateRandomSubscriberPractices()
        {
            return CreateSubscriberPracticeFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SubscriberPractice CreateRandomSubscriberPractice() =>
            CreateSubscriberPracticeFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static SubscriberPractice CreateRandomSubscriberPractice(DateTimeOffset dateTimeOffset) =>
            CreateSubscriberPracticeFiller(dateTimeOffset).Create();

        private static Filler<SubscriberPractice> CreateSubscriberPracticeFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberPractice>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberPractice => subscriberPractice.CreatedBy).Use(user)
                .OnProperty(subscriberPractice => subscriberPractice.UpdatedBy).Use(user)
                .OnProperty(subscriberPractice => subscriberPractice.SubscriberAgreement).IgnoreIt();

            return filler;
        }

        private static SubscriberPractice CreateRandomSubscriberPractice(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateSubscriberPracticeFiller(dateTimeOffset, userId).Create();

        private static Filler<SubscriberPractice> CreateSubscriberPracticeFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<SubscriberPractice>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberPractice => subscriberPractice.CreatedBy).Use(userId)
                .OnProperty(subscriberPractice => subscriberPractice.UpdatedBy).Use(userId)
                .OnProperty(subscriberPractice => subscriberPractice.SubscriberAgreement).IgnoreIt();

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