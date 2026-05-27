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
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.CoreNotDownloaded
{
    public partial class TerminologyArtifactsCoreNotDownloadedHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyArtifactsHealthItemService terminologyArtifactsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "coreNotDownloaded";
        private const string CheckNameDescription = "Core Not Downloaded";

        public TerminologyArtifactsCoreNotDownloadedHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            compareLogic = new CompareLogic();

            this.terminologyArtifactsHealthItemService = new TerminologyArtifactsCoreNotDownloadedHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static List<TerminologyArtifact> CreateRandomCoreNotDownloadedTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateCoreNotDownloadedFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateCoreNotDownloadedFiller(
            DateTimeOffset dateTimeOffset,
            string resourceType)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(a => a.IsCore).Use(true)
                .OnProperty(a => a.IsDownloaded).Use(false)
                .OnProperty(a => a.IsError).Use(false)
                .OnProperty(a => a.ResourceType).Use(resourceType)
                .OnProperty(a => a.CreatedBy).Use(user)
                .OnProperty(a => a.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
