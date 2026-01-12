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
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Services.Foundations.DataSetSpecifications;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISecurityAuditBroker> securityAuditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataSetSpecificationService dataSetSpecificationService;

        public DataSetSpecificationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.securityAuditBrokerMock = new Mock<ISecurityAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.dataSetSpecificationService = new DataSetSpecificationService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                securityAuditBroker: this.securityAuditBrokerMock.Object,
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

        private static DataSetSpecification CreateRandomModifyDataSetSpecification(
            DateTimeOffset dateTimeOffset,
            string dataSetSpecificationId)
        {
            int randomDaysInPast = GetRandomNegativeNumber();

            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification(
                dateTimeOffset,
                dataSetSpecificationId);

            randomDataSetSpecification.CreatedDate =
                randomDataSetSpecification.CreatedDate.AddDays(randomDaysInPast);

            return randomDataSetSpecification;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(DateTimeOffset dateTimeOffset) =>
            CreateDataSetSpecificationFiller(
                dateTimeOffset: dateTimeOffset,
                auditUserId: GetRandomStringWithLengthOf(255)).Create();

        private static IQueryable<DataSetSpecification> CreateRandomDataSetSpecifications()
        {
            return CreateDataSetSpecificationFiller(
                dateTimeOffset: GetRandomDateTimeOffset(),
                auditUserId: GetRandomStringWithLengthOf(255))
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DataSetSpecification CreateRandomDataSetSpecification() =>
            CreateDataSetSpecificationFiller(
                dateTimeOffset: GetRandomDateTimeOffset(),
                auditUserId: GetRandomStringWithLengthOf(255)).Create();

        private static DataSetSpecification CreateRandomDataSetSpecification(
            DateTimeOffset dateTimeOffset,
            string auditUserId) =>
            CreateDataSetSpecificationFiller(dateTimeOffset, auditUserId).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(
            DateTimeOffset dateTimeOffset,
            string auditUserId)
        {
            var filler = new Filler<DataSetSpecification>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)

                .OnProperty(dataSetSpecification => dataSetSpecification.SupplierSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.OurSpecificationVersion)
                    .Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(auditUserId)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(auditUserId)

                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SpecificationObjects).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).UseDefault()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededBy).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.PresededBy).IgnoreIt();

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