using System;
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