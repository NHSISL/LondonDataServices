using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.TerminologyArtifacts.FailedToProcessHealthCheck
{
    public partial class TerminologyArtifactsFailedToProcessHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;

        private readonly TerminologyArtifactsFailedToProcessHealthCheckService
            terminologyArtifactsFailedToProcessHealthCheckService;

        private const string CheckName = "failedToProcess";
        private const string CheckNameDescription = "Failed To Process";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;

        public TerminologyArtifactsFailedToProcessHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.terminologyArtifactsFailedToProcessHealthCheckService =
                new TerminologyArtifactsFailedToProcessHealthCheckService(
                    storageBroker: this.storageBrokerMock.Object,
                    configuration: this.configuration,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<TerminologyArtifact> CreateRandomUnhealthyTerminologyArtifacts(string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);

            return CreateTerminologyArtifactFiller(unhealthyDateTime, resourceType)
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<TerminologyArtifact> CreateRandomDegradedTerminologyArtifacts(string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);

            return CreateTerminologyArtifactFiller(degradedDateTime, resourceType)
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<TerminologyArtifact> CreateRandomHealthyTerminologyArtifacts(string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;

            return CreateTerminologyArtifactFiller(healthyDateTime, resourceType)
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<TerminologyArtifact> CreateTerminologyArtifactFiller(
            DateTimeOffset lastUpdatedDateTime,
            string resourceType)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<TerminologyArtifact>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(terminologyPoll => terminologyPoll.CreatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedBy).Use(user)
                .OnProperty(terminologyPoll => terminologyPoll.IsError).Use(true)
                .OnProperty(terminologyPoll => terminologyPoll.UpdatedDate).Use(lastUpdatedDateTime)             
                .OnProperty(terminologyPoll => terminologyPoll.ResourceType).Use(resourceType);

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int degradedCodeSystemItems = 0,
            int unHealthyCodeSystemItems = 0,
            int degradedConceptMapItems = 0,
            int unHealthyConceptMapItems = 0,
            int degradedValueSetItems = 0,
            int unHealthyValueItems = 0)
        {
            var nonHealthyItemsCount = degradedCodeSystemItems
                + unHealthyCodeSystemItems
                + degradedConceptMapItems
                + unHealthyConceptMapItems
                + degradedValueSetItems
                + unHealthyValueItems;

            var message = "Nothing to process. All up to date.";

            if (nonHealthyItemsCount > 0)
            {
                message = $"{nonHealthyItemsCount} have not been processed. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "failedToProcess", nonHealthyItemsCount },
                { "degradedCodeSystemItems", degradedCodeSystemItems },
                { "unHealthyCodeSystemItems", unHealthyCodeSystemItems },
                { "degradedConceptMapItems", degradedConceptMapItems },
                { "unHealthyConceptMapItems", unHealthyConceptMapItems },
                { "degradedValueSetItems", degradedValueSetItems },
                { "unHealthyValueItems", unHealthyValueItems },
                { "degradedThresholdMinutes", DegradedThresholdMinutes.ToString() },
                { "unHealthyThresholdMinutes", UnHealthyThresholdMinutes.ToString() },
                { "checkedAt", dateTime.ToString("o") },
                { "message", message },
                { "status", healthStatus.ToString() }
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}
