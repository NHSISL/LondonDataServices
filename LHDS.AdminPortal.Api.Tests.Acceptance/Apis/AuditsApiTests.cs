// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    [Collection(nameof(ApiTestCollection))]
    public partial class AuditsApiTests
    {
        private readonly ApiBroker apiBroker;

        public AuditsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Audit UpdateAuditWithRandomValues(Audit inputAudit)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnProperty(audit => audit.Id).Use(inputAudit.Id)
                .OnProperty(audit => audit.IngestionTrackingId).Use(inputAudit.IngestionTrackingId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(audit => audit.CreatedDate).Use(inputAudit.CreatedDate)
                .OnProperty(audit => audit.CreatedBy).Use(inputAudit.CreatedBy)
                .OnProperty(audit => audit.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<Audit> PostRandomAuditAsync(string ingestionTrackingId)
        {
            Audit randomAudit = CreateRandomAudit(ingestionTrackingId);
            await this.apiBroker.PostAuditAsync(randomAudit);

            return randomAudit;
        }

        private async ValueTask<List<Audit>> PostRandomAuditsAsync(string ingestionTrackingId)
        {
            int randomNumber = GetRandomNumber();
            var randomAudits = new List<Audit>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomAudits.Add(await PostRandomAuditAsync(ingestionTrackingId));
            }

            return randomAudits;
        }

        private static Audit CreateRandomAudit(string ingestionTrackingId) =>
            CreateRandomAuditFiller(ingestionTrackingId).Create();

        private static Filler<Audit> CreateRandomAuditFiller(string ingestionTrackingId)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(audit => audit.IngestionTrackingId).Use(ingestionTrackingId)
                .OnProperty(audit => audit.CreatedDate).Use(now)
                .OnProperty(audit => audit.CreatedBy).Use(user)
                .OnProperty(audit => audit.UpdatedDate).Use(now)
                .OnProperty(audit => audit.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync()
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateRandomIngestionTrackingFiller().Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(audit => audit.CreatedDate).Use(now)
                .OnProperty(audit => audit.CreatedBy).Use(user)
                .OnProperty(audit => audit.UpdatedDate).Use(now)
                .OnProperty(audit => audit.UpdatedBy).Use(user);

            return filler;
        }
    }
}