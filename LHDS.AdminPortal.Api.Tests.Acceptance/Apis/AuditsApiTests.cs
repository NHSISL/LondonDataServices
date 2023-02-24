using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
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
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(audit => audit.CreatedDate).Use(inputAudit.CreatedDate)
                .OnProperty(audit => audit.CreatedByUserId).Use(inputAudit.CreatedByUserId)
                .OnProperty(audit => audit.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<Audit> PostRandomAuditAsync()
        {
            Audit randomAudit = CreateRandomAudit();
            await this.apiBroker.PostAuditAsync(randomAudit);

            return randomAudit;
        }

        private async ValueTask<List<Audit>> PostRandomAuditsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomAudits = new List<Audit>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomAudits.Add(await PostRandomAuditAsync());
            }

            return randomAudits;
        }

        private static Audit CreateRandomAudit() =>
            CreateRandomAuditFiller().Create();

        private static Filler<Audit> CreateRandomAuditFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Audit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(audit => audit.CreatedDate).Use(now)
                .OnProperty(audit => audit.CreatedByUserId).Use(userId)
                .OnProperty(audit => audit.UpdatedDate).Use(now)
                .OnProperty(audit => audit.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}