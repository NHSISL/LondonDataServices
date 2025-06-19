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
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.PdsAudits;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityBroker> securityBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPdsAuditService pdsAuditService;

        public PdsAuditServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityBrokerMock = new Mock<ISecurityBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.pdsAuditService = new PdsAuditService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityBroker: this.securityBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

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

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PdsAudit CreateRandomModifyPdsAudit(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(dateTimeOffset);

            randomPdsAudit.CreatedDate =
                randomPdsAudit.CreatedDate.AddDays(randomDaysInPast);

            return randomPdsAudit;
        }

        private static PdsAudit CreateRandomModifyPdsAudit(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            PdsAudit randomPdsAudit = CreateRandomPdsAudit(dateTimeOffset, userId);

            randomPdsAudit.CreatedDate =
                randomPdsAudit.CreatedDate.AddDays(randomDaysInPast);

            return randomPdsAudit;
        }

        private static IQueryable<PdsAudit> CreateRandomPdsAudits()
        {
            return CreatePdsAuditFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static IQueryable<PdsAudit> CreateRandomPdsAuditsWithCorrelationId(Guid correlationId)
        {
            return CreatePdsAuditFiller(dateTimeOffset: GetRandomDateTimeOffset(), correlationId)
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static PdsAudit CreateRandomPdsAudit() =>
            CreatePdsAuditFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static PdsAudit CreateRandomPdsAudit(DateTimeOffset dateTimeOffset) =>
            CreatePdsAuditFiller(dateTimeOffset).Create();

        private static Filler<PdsAudit> CreatePdsAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(user)
                .OnProperty(pdsAudit => pdsAudit.UpdatedBy).Use(user);

            return filler;
        }

        private static Filler<PdsAudit> CreatePdsAuditFiller(
            DateTimeOffset dateTimeOffset,
            Guid correlationId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(user)
                .OnProperty(pdsAudit => pdsAudit.UpdatedBy).Use(user)
                .OnProperty(pdsAudit => pdsAudit.CorrelationId).Use(correlationId);

            return filler;
        }

        private static PdsAudit CreateRandomPdsAudit(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreatePdsAuditFiller(dateTimeOffset, userId).Create();

        private static Filler<PdsAudit> CreatePdsAuditFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(userId)
                .OnProperty(pdsAudit => pdsAudit.UpdatedBy).Use(userId);

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
                });
        }
    }
}