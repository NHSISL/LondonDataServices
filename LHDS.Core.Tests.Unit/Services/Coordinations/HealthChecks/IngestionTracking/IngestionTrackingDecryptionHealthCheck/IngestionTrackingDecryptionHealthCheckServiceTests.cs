using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTracking.IngestionTrackingDecryptionHealthCheck
{
    public partial class IngestionTrackingDecryptionHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IngestionTrackingDecryptionHealthCheckService ingestionTrackingDecryptionHealthCheckService;
        private const string CheckName = "decryption";

        public IngestionTrackingDecryptionHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ingestionTrackingDecryptionHealthCheckService = new IngestionTrackingDecryptionHealthCheckService(
                storageBroker: this.storageBrokerMock.Object,
                configuration: this.configuration,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> CreateRandomUnhealthyIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddDays(-5);
            return CreateIngestionTrackingFiller(unhealthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<Core.Models.Foundations.IngestionTrackings.IngestionTracking> CreateRandomDegradedIngestionTrackings()
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddDays(-1);
            return CreateIngestionTrackingFiller(unhealthyDateTime).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Filler<Core.Models.Foundations.IngestionTrackings.IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset updatedDate
        )
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Core.Models.Foundations.IngestionTrackings.IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(updatedDate)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }
    }
}
