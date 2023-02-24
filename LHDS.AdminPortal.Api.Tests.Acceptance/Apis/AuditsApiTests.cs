using System;
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