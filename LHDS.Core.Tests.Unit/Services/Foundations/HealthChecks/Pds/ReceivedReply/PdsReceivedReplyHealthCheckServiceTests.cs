using System;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.HealthChecks.PDS;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.HealthChecks.Pds.ReceivedReply
{
    public partial class PdsReceivedReplyHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly PdsReceivedReplyHealthCheckService pdsReceivedReplyHealthCheckService;
        private const string CheckName = "receivedReply";
        private const string CheckNameDescription = "Received Reply";
        private const string ConfigSectionName = "HealthChecks:Pds:ReceivedReply";
        private const int UnHealthyThresholdMinutes = 1440;

        public PdsReceivedReplyHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.pdsReceivedReplyHealthCheckService =
                new PdsReceivedReplyHealthCheckService(
                    storageBroker: this.storageBrokerMock.Object,
                    configuration: this.configuration,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<PdsAudit> CreateRandomUnhealthyPdsAudits(Guid correlationId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);
            return CreatePdsAuditFiller(unhealthyDateTime, correlationId).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<PdsAudit> CreateRandomHealthyPdsAudits(Guid correlationId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;
            return CreatePdsAuditFiller(healthyDateTime, correlationId).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<PdsAudit> CreatePdsAuditFiller(
            DateTimeOffset updatedDate,
            Guid correlationId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(updatedDate)
                .OnProperty(ingestionTracking => ingestionTracking.IsCompleted).Use(false)
                .OnProperty(ingestionTracking => ingestionTracking.CorrelationId).Use(correlationId);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}