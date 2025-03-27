// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SubscriberAgreement UpdateSubscriberAgreementWithRandomValues(
            SubscriberAgreement inputSubscriberAgreement)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberAgreement>();

            filler.Setup()
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(inputSubscriberAgreement.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnType<DateTimeOffset?>().Use(GetRandomDateTime())
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(inputSubscriberAgreement.CreatedDate)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(inputSubscriberAgreement.CreatedBy)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SubscriberAgreement> PostRandomSubscriberAgreementAsync()
        {
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();

            SubscriberAgreement storageSubscriberAgreement = 
                await this.apiBroker.PostSubscriberAgreementAsync(randomSubscriberAgreement);

            return storageSubscriberAgreement;
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
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(user)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedBy).Use(user);

            return filler;
        }
    }
}