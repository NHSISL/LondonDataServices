// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Suppliers;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.PdsAudits
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PdsAuditsApiTests
    {
        private readonly ApiBroker apiBroker;

        public PdsAuditsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PdsAudit UpdatePdsAuditWithRandomValues(PdsAudit inputPdsAudit)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<PdsAudit>();

            filler.Setup()
                .OnProperty(pdsAudit => pdsAudit.Id).Use(inputPdsAudit.Id)
                .OnProperty(pdsAudit => pdsAudit.CorrelationId).Use(inputPdsAudit.CorrelationId)
                .OnProperty(pdsAudit => pdsAudit.FileName).Use(inputPdsAudit.FileName)
                .OnProperty(pdsAudit => pdsAudit.Message).Use(inputPdsAudit.Message)
                .OnProperty(pdsAudit => pdsAudit.MessageId).Use(inputPdsAudit.Message)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(inputPdsAudit.CreatedBy)
                .OnProperty(pdsAudit => pdsAudit.CreatedDate).Use(inputPdsAudit.CreatedDate)
                .OnProperty(pdsAudit => pdsAudit.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }


        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync(Guid supplierId)
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking(supplierId);
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static List<PdsAudit> CreateRandomPdsAudits()
        {
            return CreatePdsAuditFiller()
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private static PdsAudit CreateRandomPdsAudit() =>
            CreatePdsAuditFiller().Create();

        private static Filler<PdsAudit> CreatePdsAuditFiller()
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsAudit>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(pdsAudit => pdsAudit.CreatedBy).Use(user)
                .OnProperty(pdsAudit => pdsAudit.UpdatedBy).Use(user);

            return filler;
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