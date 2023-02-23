using System;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackings
{
    [Collection(nameof(ApiTestCollection))]
    public partial class IngestionTrackingsApiTests
    {
        private readonly ApiBroker apiBroker;

        public IngestionTrackingsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateRandomIngestionTrackingFiller().Create();

        private static Filler<IngestionTracking> CreateRandomIngestionTrackingFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedByUserId).Use(userId)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedDate).Use(now)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}