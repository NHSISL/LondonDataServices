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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.FailedToProcess
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyArtifactsHealthItemService terminologyPollsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "artifactErrors";
        private const string CheckNameDescription = "Artifact Errors";
        private const string ConfigSectionName = "HealthChecks:TerminologyArtifacts:FailedToProcess";

        public TerminologyArtifactsFailedToProcessHealthCheckServiceTests()
        {
            storageBrokerMock = new Mock<IStorageBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            loggingBrokerMock = new Mock<ILoggingBroker>();

            var appSettingsStub = new Dictionary<string, string> {
                {$"{ConfigSectionName}:DegradedThreshold", "1440"},
                {$"{ConfigSectionName}:UnHealthyThreshold", "2880"},
            };

            this.inMemoryConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            compareLogic = new CompareLogic();

            this.terminologyPollsHealthItemService = new TerminologyArtifactsFailedToProcessHealthCheckService(
                storageBroker: storageBrokerMock.Object,
                configuration: inMemoryConfiguration,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<HealthCheckResult, bool>> SameHealthCheckResultAs(
            HealthCheckResult expectedHealthCheckResult) =>
                actualHealthCheckResult => compareLogic.Compare(expectedHealthCheckResult, actualHealthCheckResult)
                    .AreEqual;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<TerminologyArtifact> CreateRandomTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateTerminologyArtifactsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactsFiller(
            DateTimeOffset updatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyPolls => terminologyPolls.UpdatedDate).Use(updatedDateTime)
                .OnProperty(terminologyPolls => terminologyPolls.ResourceType).Use(resourceType)
                .OnProperty(terminologyPolls => terminologyPolls.IsError).Use(true)
                .OnProperty(terminologyPolls => terminologyPolls.CreatedBy).Use(user)
                .OnProperty(terminologyPolls => terminologyPolls.UpdatedBy).Use(user);

            return filler;
        }

        private static List<TerminologyArtifact> CreateRandomNonErrorTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateNonErrorTerminologyArtifactsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateNonErrorTerminologyArtifactsFiller(
            DateTimeOffset updatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyPolls => terminologyPolls.UpdatedDate).Use(updatedDateTime)
                .OnProperty(terminologyPolls => terminologyPolls.ResourceType).Use(resourceType)
                .OnProperty(terminologyPolls => terminologyPolls.IsError).Use(false)
                .OnProperty(terminologyPolls => terminologyPolls.CreatedBy).Use(user)
                .OnProperty(terminologyPolls => terminologyPolls.UpdatedBy).Use(user);

            return filler;
        }
    }
}
