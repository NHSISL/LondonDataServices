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
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.IngestionTrackings.PipelineAlive
{
    public partial class IngestionTrackingPipelineAliveHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IIngestionTrackingHealthItemService ingestionTrackingHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "pipelineAlive";
        private const string CheckDescriptionName = "Pipeline Alive";
        private const string ConfigSectionName = "HealthChecks:IngestionTracking:PipelineAlive";

        public IngestionTrackingPipelineAliveHealthCheckServiceTests()
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

            this.ingestionTrackingHealthItemService = new IngestionTrackingPipelineAliveHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<IngestionTracking> CreateRandomIngestionTrackings(
            DateTimeOffset createdDate,
            int count)
        {
            return CreateIngestionTrackingFiller(createdDate)
                .Create(count)
                    .ToList();
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset createdDate)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(createdDate)
                .OnType<DateTimeOffset?>().Use(createdDate)
                .OnProperty(i => i.CreatedBy).Use(user)
                .OnProperty(i => i.UpdatedBy).Use(user)
                .OnProperty(i => i.CreatedDate).Use(createdDate);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
