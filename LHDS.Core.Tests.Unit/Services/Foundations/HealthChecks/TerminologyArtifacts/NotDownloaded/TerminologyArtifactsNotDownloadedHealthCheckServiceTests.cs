// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Services.Foundations.HealthChecks.TerminologyArtifacts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.TerminologyArtifacts.NotDownloaded
{
    public partial class TerminologyArtifactsNotDownloadedHealthCheckServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IConfiguration inMemoryConfiguration;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ITerminologyArtifactsHealthItemService terminologyArtifactsHealthItemService;
        private readonly ICompareLogic compareLogic;
        private const string CheckName = "notDownloaded";
        private const string CheckNameDescription = "Not Downloaded";
        private const string ConfigSectionName = "HealthChecks:TerminologyArtifacts:NotDownloaded";

        public TerminologyArtifactsNotDownloadedHealthCheckServiceTests()
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

            this.terminologyArtifactsHealthItemService = new TerminologyArtifactsNotDownloadedHealthCheckService(
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

        private static SqlException GetSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<TerminologyArtifact> CreateRandomNotDownloadedTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateNotDownloadedTerminologyArtifactsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateNotDownloadedTerminologyArtifactsFiller(
            DateTimeOffset updatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedDate).Use(updatedDateTime)
                .OnProperty(terminologyArtifact => terminologyArtifact.ResourceType).Use(resourceType)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsError).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }

        private static List<TerminologyArtifact> CreateRandomDownloadedTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateDownloadedTerminologyArtifactsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateDownloadedTerminologyArtifactsFiller(
            DateTimeOffset updatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedDate).Use(updatedDateTime)
                .OnProperty(terminologyArtifact => terminologyArtifact.ResourceType).Use(resourceType)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(true)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsError).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }

        private static List<TerminologyArtifact> CreateRandomErrorTerminologyArtifacts(
            DateTimeOffset dateTimeOffset,
            string resourceType,
            int count)
        {
            return CreateErrorTerminologyArtifactsFiller(dateTimeOffset, resourceType)
                .Create(count)
                    .ToList();
        }

        private static Filler<TerminologyArtifact> CreateErrorTerminologyArtifactsFiller(
            DateTimeOffset updatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedDate).Use(updatedDateTime)
                .OnProperty(terminologyArtifact => terminologyArtifact.ResourceType).Use(resourceType)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsDownloaded).Use(false)
                .OnProperty(terminologyArtifact => terminologyArtifact.IsError).Use(true)
                .OnProperty(terminologyArtifact => terminologyArtifact.CreatedBy).Use(user)
                .OnProperty(terminologyArtifact => terminologyArtifact.UpdatedBy).Use(user);

            return filler;
        }
    }
}
