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

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.ResolvedAddresses.PipelineAlive
{
    public partial class ResolvedAddressPipelineAliveHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IResolvedAddressHealthItemService resolvedAddressHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "pipelineAlive";
        private const string CheckDescriptionName = "Pipeline Alive";
        private const string ConfigSectionName = "HealthChecks:ResolvedAddress:PipelineAlive";

        public ResolvedAddressPipelineAliveHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string>
            {
                { $"{ConfigSectionName}:DegradedThreshold", "1440" },
                { $"{ConfigSectionName}:UnHealthyThreshold", "2880" },
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.resolvedAddressHealthItemService = new ResolvedAddressPipelineAliveHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<ResolvedAddress> CreateRandomResolvedAddresses(
            DateTimeOffset createdDate,
            int count)
        {
            return CreateResolvedAddressFiller(createdDate)
                .Create(count)
                    .ToList();
        }

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset createdDate)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(createdDate)
                .OnType<DateTimeOffset?>().Use(createdDate)
                .OnProperty(r => r.CreatedBy).Use(user)
                .OnProperty(r => r.UpdatedBy).Use(user)
                .OnProperty(r => r.CreatedDate).Use(createdDate)
                .OnProperty(r => r.HashedUnstructuredPostalAddress).Use(new char[32]);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
