// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
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

        private static SubscriberCredential UpdateSubscriberCredentialWithRandomValues(
            SubscriberCredential inputSubscriberCredential)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnProperty(subscriberCredential => subscriberCredential.Id)
                    .Use(inputSubscriberCredential.Id)

                .OnType<DateTimeOffset>().Use(GetRandomDateTime())

                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate)
                    .Use(inputSubscriberCredential.CreatedDate)

                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy)
                    .Use(inputSubscriberCredential.CreatedBy)

                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SubscriberCredential> PostRandomSubscriberCredentialAsync(Guid id)
        {
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential(id);
            await this.apiBroker.PostSubscriberCredentialAsync(randomSubscriberCredential);

            return randomSubscriberCredential;
        }

        private async ValueTask<List<SubscriberCredential>> PostRandomSubscriberCredentialsAsync(Guid id)
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberCredentials = new List<SubscriberCredential>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSubscriberCredentials.Add(await PostRandomSubscriberCredentialAsync(id));
            }

            return randomSubscriberCredentials;
        }

        private static SubscriberCredential CreateRandomSubscriberCredential(Guid id) =>
            CreateRandomSubscriberCredentialFiller(id).Create();

        private static Filler<SubscriberCredential> CreateRandomSubscriberCredentialFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberCredential>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.Id).Use(id)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.CreatedBy).Use(user)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedDate).Use(now)
                .OnProperty(subscriberCredential => subscriberCredential.UpdatedBy).Use(user);

            return filler;
        }

        private async ValueTask<SubscriberAgreement> PostRandomSubscriberAgreementAsync(Guid id)
        {
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement(id);
            await this.apiBroker.PostSubscriberAgreementAsync(randomSubscriberAgreement);

            return randomSubscriberAgreement;
        }

        private async ValueTask<List<SubscriberAgreement>> PostRandomSubscriberAgreementsAsync(Guid id)
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberAgreements = new List<SubscriberAgreement>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSubscriberAgreements.Add(await PostRandomSubscriberAgreementAsync(id));
            }

            return randomSubscriberAgreements;
        }

        private static SubscriberAgreement CreateRandomSubscriberAgreement(Guid id) =>
            CreateRandomSubscriberAgreementFiller(id).Create();

        private static Filler<SubscriberAgreement> CreateRandomSubscriberAgreementFiller(Guid id)
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(id)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}