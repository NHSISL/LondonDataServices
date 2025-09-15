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
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.ResolvedAddresses;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityAuditBroker> securityAuditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly CompareLogic compareLogic;

        public ResolvedAddressServiceTests()
        {
            this.compareLogic = new CompareLogic();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityAuditBrokerMock = new Mock<ISecurityAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.auditBrokerMock = new Mock<IAuditBroker>();

            this.resolvedAddressService = new ResolvedAddressService(
                storageBroker: this.storageBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityAuditBroker: this.securityAuditBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                auditBroker: this.auditBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<List<ResolvedAddress>, bool>> SameResolvedAddressesAs(
            List<ResolvedAddress> expectedResolvedAddresses)
        {
            return actualResolvedAddresses =>
                this.compareLogic.Compare(expectedResolvedAddresses, actualResolvedAddresses)
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

        private static ResolvedAddress CreateRandomModifyResolvedAddress(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(dateTimeOffset);

            randomResolvedAddress.CreatedDate =
                randomResolvedAddress.CreatedDate.AddDays(randomDaysInPast);

            return randomResolvedAddress;
        }

        private static ResolvedAddress CreateRandomModifyResolvedAddress(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(dateTimeOffset, userId);
            randomResolvedAddress.CreatedDate = randomResolvedAddress.CreatedDate.AddDays(randomDaysInPast);

            return randomResolvedAddress;
        }

        private static IQueryable<ResolvedAddress> CreateRandomResolvedAddresses()
        {
            return CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static ResolvedAddress CreateRandomResolvedAddress() =>
            CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ResolvedAddress CreateRandomResolvedAddress(DateTimeOffset dateTimeOffset) =>
            CreateResolvedAddressFiller(dateTimeOffset).Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }

        private static ResolvedAddress CreateRandomResolvedAddress(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateResolvedAddressFiller(dateTimeOffset, userId).Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(userId)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(userId);

            return filler;
        }

        private EntraUser CreateRandomEntraUser(string randomEntraUserId = "")
        {
            var userId = string.IsNullOrWhiteSpace(randomEntraUserId) ? 
                GetRandomStringWithLengthOf(255) : randomEntraUserId;

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