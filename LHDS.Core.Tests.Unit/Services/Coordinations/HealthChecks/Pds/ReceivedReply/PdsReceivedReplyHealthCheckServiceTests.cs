using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Services.Foundations.HealthChecks.PDS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.Pds.ReceivedReply
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

        private static IQueryable<PdsAudit> CreateRandomUnhealthyPdsAudits(Guid correlationId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);
            return CreatePdsAuditFiller(unhealthyDateTime, correlationId).Create(count: 1).AsQueryable();
        }

        private static IQueryable<PdsAudit> CreateRandomHealthyPdsAudits(Guid correlationId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;
            return CreatePdsAuditFiller(healthyDateTime, correlationId).Create(count: 1).AsQueryable();
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

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            int unhealthyItemsCount = 0)
        {
            var totalItemsCount = unhealthyItemsCount;
            var message = "All requests received reply.";

            if (totalItemsCount > 0)
            {
                message = $"{totalItemsCount} requests have no reply. Please check logs and function status.";
            }

            return new Dictionary<string, object>
            {
                { "description", CheckNameDescription },
                { "notReceivedReply", totalItemsCount},
                { "unHealthyItems", unhealthyItemsCount},
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
