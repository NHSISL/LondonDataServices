// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Services.Foundations.HealthChecks.IngestionTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.HealthChecks.IngestionTrackings.FilesReceivedHealthCheck
{
    public partial class IngestionTrackingFilesReceivedHealthCheckServiceTests
    {
        private readonly IConfiguration configuration;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IngestionTrackingFilesReceivedHealthCheckService ingestionTrackingFilesReceivedHealthCheckService;
        private const string CheckName = "filesReceived";
        private const int DegradedThresholdMinutes = 1440;
        private const int UnHealthyThresholdMinutes = 2880;

        public IngestionTrackingFilesReceivedHealthCheckServiceTests()
        {
            this.configuration = new ConfigurationBuilder().Build();
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.ingestionTrackingFilesReceivedHealthCheckService =
                new IngestionTrackingFilesReceivedHealthCheckService(
                    storageBroker: this.storageBrokerMock.Object,
                    configuration: this.configuration,
                    dateTimeBroker: this.dateTimeBrokerMock.Object,
                    loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static IQueryable<Supplier> CreateRandomIngestionTrackedSupplier()
        {
            return CreateSupplierFiller(true).Create(count: 1).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomUnhealthyIngestionTrackings(Guid supplierId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset unhealthyDateTime = dateTimeOffset.AddMinutes((UnHealthyThresholdMinutes + 1) * -1);

            return CreateIngestionTrackingFiller(unhealthyDateTime, supplierId).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomDegradedIngestionTrackings(Guid supplierId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset degradedDateTime = dateTimeOffset.AddMinutes((DegradedThresholdMinutes + 1) * -1);

            return CreateIngestionTrackingFiller(degradedDateTime, supplierId).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static IQueryable<IngestionTracking> CreateRandomHealthyIngestionTrackings(Guid supplierId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset healthyDateTime = dateTimeOffset;

            return CreateIngestionTrackingFiller(healthyDateTime, supplierId).Create(count: GetRandomNumber()).AsQueryable();
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset createdDate,
            Guid supplierId)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(createdDate)
                .OnProperty(ingestionTracking => ingestionTracking.RetryCount).Use(3)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt();

            return filler;
        }

        private static Filler<Supplier> CreateSupplierFiller(
            bool isIngestionTracked)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            string user = Guid.NewGuid().ToString();
            Guid supplierId = Guid.NewGuid();
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(supplier => supplier.CreatedBy).Use(user)
                .OnProperty(supplier => supplier.UpdatedBy).Use(user)
                .OnProperty(supplier => supplier.Id).Use(supplierId)
                .OnProperty(supplier => supplier.IsIngestionTracked).Use(isIngestionTracked)
                .OnProperty(supplier => supplier.DataSets).IgnoreIt()
                .OnProperty(supplier => supplier.IngestionTrackings).IgnoreIt();

            return filler;
        }

        private static Dictionary<string, object> GetHealthCheckResultValues(
            DateTimeOffset dateTime,
            HealthStatus healthStatus,
            string supplierName = "",
            int expectedItemsCount = 0)
        {
            var supplierFilesResult = new Dictionary<string, object>
            {
                { "description", supplierName },
                { "filesReceived", expectedItemsCount},
                { "degradedThresholdMinutes", 1440.ToString() },
                { "unHealthyThresholdMinutes", 2880.ToString() },
                { "checkedAt", dateTime.ToString("o") },
                { "status", healthStatus.ToString() }
            };

            return new Dictionary<string, object>
            {
                { "description", "Files Received" },
                { "checkedAt", dateTime.ToString("o") },
                { supplierName, supplierFilesResult },
                { "filesReceived", expectedItemsCount},
                { "status", healthStatus.ToString() }
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}
