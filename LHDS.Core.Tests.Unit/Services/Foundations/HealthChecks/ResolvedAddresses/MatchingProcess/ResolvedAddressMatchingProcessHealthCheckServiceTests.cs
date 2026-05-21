using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.MatchingProcess
{
    public partial class ResolvedAddressMatchingProcessHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IResolvedAddressHealthItemService resolvedAddressHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "addressMatchingTime";
        private const string CheckDescriptionName = "Address Matching Time";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:AddressMatchingTime";

        public ResolvedAddressMatchingProcessHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string> {
                {$"{ConfigSectionName}:DegradedThreshold", "15"},
                {$"{ConfigSectionName}:UnHealthyThreshold", "30"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.resolvedAddressHealthItemService = new ResolvedAddressMatchingProcessHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
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

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static List<Audit> CreateRandomResolvedAddressMatchAudits(
            DateTimeOffset dateTimeOffset,
            int count)
        {
            return CreateAuditFiller(dateTimeOffset)
                .Create(count)
                    .ToList();
        }

        private static Filler<Audit> CreateAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(audit => audit.AuditType).Use("Resolved Address Match")
                .OnProperty(audit => audit.CreatedDate).Use(dateTimeOffset)
                .OnProperty(audit => audit.CreatedBy).Use(user)
                .OnProperty(audit => audit.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
