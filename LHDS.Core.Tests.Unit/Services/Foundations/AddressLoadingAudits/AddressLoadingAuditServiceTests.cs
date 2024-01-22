// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Services.Foundations.AddressLoadingAudits;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAddressLoadingAuditService addressLoadingAuditService;

        public AddressLoadingAuditServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressLoadingAuditService = new AddressLoadingAuditService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
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

        private static AddressLoadingAudit CreateRandomModifyAddressLoadingAudit(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(dateTimeOffset);

            randomAddressLoadingAudit.CreatedDate =
                randomAddressLoadingAudit.CreatedDate.AddDays(randomDaysInPast);

            return randomAddressLoadingAudit;
        }

        private static IQueryable<AddressLoadingAudit> CreateRandomAddressLoadingAudits()
        {
            return CreateAddressLoadingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static AddressLoadingAudit CreateRandomAddressLoadingAudit() =>
            CreateAddressLoadingAuditFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static AddressLoadingAudit CreateRandomAddressLoadingAudit(DateTimeOffset dateTimeOffset) =>
            CreateAddressLoadingAuditFiller(dateTimeOffset).Create();

        private static Filler<AddressLoadingAudit> CreateAddressLoadingAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AddressLoadingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.CreatedBy).Use(user)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.UpdatedBy).Use(user);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}