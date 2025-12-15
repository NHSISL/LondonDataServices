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
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityBroker> securityBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISubscriberAgreementService subscriberAgreementService;

        public SubscriberAgreementServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityBrokerMock = new Mock<ISecurityBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.subscriberAgreementService = new SubscriberAgreementService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityBroker: this.securityBrokerMock.Object,
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

        private static SubscriberAgreement CreateRandomModifySubscriberAgreement(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(dateTimeOffset);

            randomSubscriberAgreement.CreatedDate =
                randomSubscriberAgreement.CreatedDate.AddDays(randomDaysInPast);

            return randomSubscriberAgreement;
        }

        private static SubscriberAgreement CreateRandomModifySubscriberAgreement(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(dateTimeOffset, userId);

            randomSubscriberAgreement.CreatedDate =
                randomSubscriberAgreement.CreatedDate.AddDays(randomDaysInPast);

            return randomSubscriberAgreement;
        }

        private static IQueryable<SubscriberAgreement> CreateRandomSubscriberAgreements()
        {
            return CreateSubscriberAgreementFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement() =>
            CreateSubscriberAgreementFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static SubscriberAgreement CreateRandomSubscriberAgreement(DateTimeOffset dateTimeOffset) =>
            CreateSubscriberAgreementFiller(dateTimeOffset).Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.IngestionTrackings).IgnoreIt();

            return filler;
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateSubscriberAgreementFiller(dateTimeOffset, userId).Create();

        private static Filler<SubscriberAgreement> CreateSubscriberAgreementFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(userId)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(userId)
                .OnProperty(subscriberAgreement => subscriberAgreement.IngestionTrackings).IgnoreIt();

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