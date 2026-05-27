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
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.HealthChecks.PDS;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.PdsAudits.PipelineAlive
{
    public partial class PdsAuditPipelineAliveHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IPdsHealthItemService pdsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "pipelineAlive";
        private const string CheckDescriptionName = "Pipeline Alive";
        private const string ConfigSectionName = "HealthChecks:PdsAudit:PipelineAlive";

        public PdsAuditPipelineAliveHealthCheckServiceTests()
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

            this.pdsHealthItemService = new PdsAuditPipelineAliveHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<PdsAudit> CreateRandomPdsAudits(DateTimeOffset dateTimeOffset, int count)
        {
            return CreatePdsAuditFiller(dateTimeOffset)
                .Create(count)
                    .ToList();
        }

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

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
