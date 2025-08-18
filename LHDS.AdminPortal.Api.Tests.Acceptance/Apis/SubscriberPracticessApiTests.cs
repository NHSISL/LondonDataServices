// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberPractices
{
    [Collection(nameof(ApiTestCollection))]
    public partial class SubscriberPracticesApiTests
    {
        private readonly ApiBroker apiBroker;

        public SubscriberPracticesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SubscriberPractice UpdateSubscriberPracticeWithRandomValues(
            SubscriberPractice inputSubscriberPractice)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<SubscriberPractice>();

            filler.Setup()
                .OnProperty(subscriberAgreement => subscriberAgreement.Id).Use(inputSubscriberPractice.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnType<DateTimeOffset?>().Use(GetRandomDateTime())
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedDate).Use(inputSubscriberPractice.CreatedDate)
                .OnProperty(subscriberAgreement => subscriberAgreement.CreatedBy).Use(inputSubscriberPractice.CreatedBy)
                .OnProperty(subscriberAgreement => subscriberAgreement.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<SubscriberPractice> PostRandomSubscriberPracticeAsync()
        {
            SubscriberPractice randomSubscriberPractice = CreateRandomSubscriberPractice();

            SubscriberPractice storageSubscriberPractice =
                await this.apiBroker.PostSubscriberPracticeAsync(randomSubscriberPractice);

            return storageSubscriberPractice;
        }

        private async ValueTask<List<SubscriberPractice>> PostRandomSubscriberPracticesAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomSubscriberPractices = new List<SubscriberPractice>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomSubscriberPractices.Add(await PostRandomSubscriberPracticeAsync());
            }

            return randomSubscriberPractices;
        }

        private static SubscriberPractice CreateRandomSubscriberPractice() =>
            CreateRandomSubscriberPracticeFiller().Create();

        private static Filler<SubscriberPractice> CreateRandomSubscriberPracticeFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<SubscriberPractice>();

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