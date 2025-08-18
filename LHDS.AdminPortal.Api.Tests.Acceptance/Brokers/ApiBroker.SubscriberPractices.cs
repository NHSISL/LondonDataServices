// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberPractices;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string SubscriberPracticesRelativeUrl = "api/subscriberPractices";

        public async ValueTask<SubscriberPractice> PostSubscriberPracticeAsync(SubscriberPractice subscriberPractice) =>
            await this.apiFactoryClient.PostContentAsync(SubscriberPracticesRelativeUrl, subscriberPractice);

        public async ValueTask<SubscriberPractice> GetSubscriberPracticeByIdAsync(Guid subscriberPracticeId) =>
            await this.apiFactoryClient.GetContentAsync<SubscriberPractice>($"{SubscriberPracticesRelativeUrl}/{subscriberPracticeId}");

        public async ValueTask<List<SubscriberPractice>> GetAllSubscriberPracticesAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<SubscriberPractice>>($"{SubscriberPracticesRelativeUrl}/");

        public async ValueTask<SubscriberPractice> PutSubscriberPracticeAsync(SubscriberPractice subscriberPractice) =>
            await this.apiFactoryClient.PutContentAsync(SubscriberPracticesRelativeUrl, subscriberPractice);

        public async ValueTask<SubscriberPractice> DeleteSubscriberPracticeByIdAsync(Guid subscriberPracticeId) =>
            await this.apiFactoryClient.DeleteContentAsync<SubscriberPractice>($"{SubscriberPracticesRelativeUrl}/{subscriberPracticeId}");
    }
}
