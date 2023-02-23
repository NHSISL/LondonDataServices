using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<IngestionTracking> PostRandomIngestionTrackingAsync()
        {
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            await this.apiBroker.PostIngestionTrackingAsync(randomIngestionTracking);

            return randomIngestionTracking;
        }

        private async ValueTask<List<IngestionTracking>> PostRandomIngestionTrackingsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomIngestionTrackings = new List<IngestionTracking>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomIngestionTrackings.Add(await PostRandomIngestionTrackingAsync());
            }

            return randomIngestionTrackings;
        }

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