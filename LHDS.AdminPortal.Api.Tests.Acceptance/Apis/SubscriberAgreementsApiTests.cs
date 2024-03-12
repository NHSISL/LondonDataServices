using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberAgreements
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SubscriberAgreementsApiTests
    {
        private readonly ApiBroker apiBroker;

        public SubscriberAgreementsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<SubscriberAgreement> PostRandomSubscriberAgreementAsync()
        {
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            await this.apiBroker.PostSubscriberAgreementAsync(randomSubscriberAgreement);

            return randomSubscriberAgreement;
        }

        private async ValueTask<List<SubscriberAgreement>> PostRandomSubscriberAgreementsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberAgreements = new List<SubscriberAgreement>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSubscriberAgreements.Add(await PostRandomSubscriberAgreementAsync());
            }

            return randomSubscriberAgreements;
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement() =>
            CreateRandomSubscriberAgreementFiller().Create();

        private static Filler<SubscriberAgreement> CreateRandomSubscriberAgreementFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}