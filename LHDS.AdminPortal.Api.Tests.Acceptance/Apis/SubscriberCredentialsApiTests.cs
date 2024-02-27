using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SubscriberCredentialsApiTests
    {
        private readonly ApiBroker apiBroker;

        public SubscriberCredentialsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SubscriberCredential UpdateSubscriberCredentialWithRandomValues(SubscriberCredential inputSubscriberCredential)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(inputSubscriberCredential.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate).Use(inputSubscriberCredential.CreatedDate)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(inputSubscriberCredential.CreatedBy)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SubscriberCredential> PostRandomSubscriberCredentialAsync()
        {
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            await this.apiBroker.PostSubscriberCredentialAsync(randomSubscriberCredential);

            return randomSubscriberCredential;
        }

        private async ValueTask<List<SubscriberCredential>> PostRandomSubscriberCredentialsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberCredentials = new List<SubscriberCredential>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSubscriberCredentials.Add(await PostRandomSubscriberCredentialAsync());
            }

            return randomSubscriberCredentials;
        }

        private static SubscriberCredential CreateRandomSubscriberCredential() =>
            CreateRandomSubscriberCredentialFiller().Create();

        private static Filler<SubscriberCredential> CreateRandomSubscriberCredentialFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }
    }
}