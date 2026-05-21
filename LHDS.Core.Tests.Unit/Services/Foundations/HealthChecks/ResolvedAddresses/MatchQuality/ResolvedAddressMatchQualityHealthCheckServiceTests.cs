// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.HealthChecks.ResolvedAddress;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses
{
    public partial class ResolvedAddressMatchQualityHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IResolvedAddressHealthItemService resolvedAddressHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "matchQuality";
        private const string CheckDescriptionName = "Match Quality";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:MatchQuality";

        public ResolvedAddressMatchQualityHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string> {
                {$"{ConfigSectionName}:DegradedThresholdPercentage", "0.9"},
                {$"{ConfigSectionName}:UnHealthyThresholdPercentage", "0.8"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.resolvedAddressHealthItemService = new ResolvedAddressMatchQualityHealthCheckService(
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

        private static List<ResolvedAddress> CreateRandomResolvedAddresses(
            DateTimeOffset dateTimeOffset,
            double percentageToMatch)
        {
            List<ResolvedAddress> resolvedAddreses = new List<ResolvedAddress>();
            int countToMatch = (int) (percentageToMatch * 10);
            int countToNotMatch = 10 - countToMatch;

            List<ResolvedAddress> matchingResolvedAddress = CreateResolvedAddressFiller(dateTimeOffset, true)
                .Create(countToMatch)
                    .ToList();

            List<ResolvedAddress> unMatchingResolvedAddress = CreateResolvedAddressFiller(dateTimeOffset, false)
                .Create(countToNotMatch)
                    .ToList();

            resolvedAddreses.AddRange(matchingResolvedAddress);
            resolvedAddreses.AddRange(unMatchingResolvedAddress);

            return resolvedAddreses;
        }

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(
            DateTimeOffset dateTimeOffset,
            bool isMatched)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.MatchedWithAssign).Use(isMatched)
                .OnProperty(resolvedAddress => resolvedAddress.HashedUnstructuredPostalAddress).Use(new char[32])
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}