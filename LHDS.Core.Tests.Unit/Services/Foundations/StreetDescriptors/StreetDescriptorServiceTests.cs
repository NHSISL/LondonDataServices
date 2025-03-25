// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.StreetDescriptors;
using LHDS.Core.Services.Foundations.StreetDescriptors;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.StreetDescriptors
{
    public partial class StreetDescriptorServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityBroker> securityBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly IStreetDescriptorService streetDescriptorService;
        private readonly CompareLogic compareLogic;

        public StreetDescriptorServiceTests()
        {
            this.compareLogic = new CompareLogic();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityBrokerMock = new Mock<ISecurityBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.auditBrokerMock = new Mock<IAuditBroker>();

            this.streetDescriptorService = new StreetDescriptorService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityBroker: this.securityBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                auditBroker: this.auditBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<List<StreetDescriptor>, bool>> SameStreetDescriptorsAs(List<StreetDescriptor> expectedStreetDescriptors)
        {
            return actualStreetDescriptors =>
                this.compareLogic.Compare(expectedStreetDescriptors, actualStreetDescriptors)
                    .AreEqual;
        }

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

        private static StreetDescriptor CreateRandomModifyStreetDescriptor(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            StreetDescriptor randomStreetDescriptor = CreateRandomStreetDescriptor(dateTimeOffset);

            randomStreetDescriptor.CreatedDate =
                randomStreetDescriptor.CreatedDate.AddDays(randomDaysInPast);

            return randomStreetDescriptor;
        }

        private static StreetDescriptor CreateRandomModifyStreetDescriptor(DateTimeOffset dateTimeOffset, string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            StreetDescriptor randomStreetDescriptor = CreateRandomStreetDescriptor(dateTimeOffset, userId);

            randomStreetDescriptor.CreatedDate =
                randomStreetDescriptor.CreatedDate.AddDays(randomDaysInPast);

            return randomStreetDescriptor;
        }

        private static IQueryable<StreetDescriptor> CreateRandomStreetDescriptors()
        {
            return CreateStreetDescriptorFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static StreetDescriptor CreateRandomStreetDescriptor() =>
            CreateStreetDescriptorFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static StreetDescriptor CreateRandomStreetDescriptor(DateTimeOffset dateTimeOffset) =>
            CreateStreetDescriptorFiller(dateTimeOffset).Create();

        private static Filler<StreetDescriptor> CreateStreetDescriptorFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<StreetDescriptor>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(streetDescriptor => streetDescriptor.CreatedBy).Use(user)
                .OnProperty(streetDescriptor => streetDescriptor.UpdatedBy).Use(user);

            return filler;
        }

        private static StreetDescriptor CreateRandomStreetDescriptor(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateStreetDescriptorFiller(dateTimeOffset, userId).Create();

        private static Filler<StreetDescriptor> CreateStreetDescriptorFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<StreetDescriptor>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(streetDescriptor => streetDescriptor.CreatedBy).Use(userId)
                .OnProperty(streetDescriptor => streetDescriptor.UpdatedBy).Use(userId);

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