// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackingAudits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackingAudits
{
    [Collection(nameof(ApiTestCollection))]
    public partial class IngestionTrackingAuditsApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly ITestOutputHelper output;

        public IngestionTrackingAuditsApiTests(ApiBroker apiBroker, ITestOutputHelper output)
        {
            this.apiBroker = apiBroker;
            this.output = output;
        }

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IngestionTrackingAudit UpdateIngestionTrackingAuditWithRandomValues(IngestionTrackingAudit inputIngestionTrackingAudit)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<IngestionTrackingAudit>();

            filler.Setup()
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.Id).Use(inputIngestionTrackingAudit.Id)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTrackingId).Use(inputIngestionTrackingAudit.IngestionTrackingId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.CreatedDate).Use(inputIngestionTrackingAudit.CreatedDate)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.CreatedBy).Use(inputIngestionTrackingAudit.CreatedBy)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedDate).Use(now);
            return filler.Create();
        }

        private async ValueTask<IngestionTrackingAudit> PostRandomIngestionTrackingAuditAsync(Guid ingestionTrackingId)
        {
            IngestionTrackingAudit randomAudit = CreateRandomIngestionTrackingAudit(ingestionTrackingId);

            IngestionTrackingAudit storageAudit =
                await this.apiBroker.PostIngestionTrackingAuditAsync(randomAudit);

            return storageAudit;
        }

        private async ValueTask<List<IngestionTrackingAudit>> PostRandomIngestionTrackingAuditsAsync(Guid ingestionTrackingId)
        {
            int randomNumber = GetRandomNumber();
            var randomAudits = new List<IngestionTrackingAudit>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomAudits.Add(await PostRandomIngestionTrackingAuditAsync(ingestionTrackingId));
            }

            return randomAudits;
        }

        private static IngestionTrackingAudit CreateRandomIngestionTrackingAudit(Guid ingestionTrackingId) =>
            CreateRandomIngestionTrackingAuditFiller(ingestionTrackingId).Create();

        private static Filler<IngestionTrackingAudit> CreateRandomIngestionTrackingAuditFiller(Guid ingestionTrackingId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTrackingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.IngestionTrackingId).Use(ingestionTrackingId)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.CreatedDate).Use(now)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.CreatedBy).Use(user)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedDate).Use(now)
                .OnProperty(ingestionTrackingAudit => ingestionTrackingAudit.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);

            IngestionTracking storageIngestionTracking =
                await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return storageIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking(Guid supplierId) =>
            CreateRandomIngestionTrackingFiller(supplierId).Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreementId).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<Supplier> PostRandomSupplierAsync()
        {
            Supplier randomSupplier = CreateRandomSupplier();
            await this.apiBroker.PostSupplierAsync(randomSupplier);

            return randomSupplier;
        }

        private static Supplier CreateRandomSupplier() =>
            CreateRandomSupplierFiller().Create();

        private static Filler<Supplier> CreateRandomSupplierFiller()
        {
            string userId = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Supplier>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(supplier => supplier.CreatedDate).Use(now)
                .OnProperty(supplier => supplier.CreatedBy).Use(userId)
                .OnProperty(supplier => supplier.UpdatedDate).Use(now)
                .OnProperty(supplier => supplier.UpdatedBy).Use(userId);

            return filler;
        }
    }
}