// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string SubscriberAgreementsRelativeUrl = "api/subscriberAgreements";
        private const string subscriberAgreementsRelativeOdataUrl = "odata/subscriberAgreements";

        public async ValueTask<SubscriberAgreement> PostSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            await this.apiFactoryClient.PostContentAsync(SubscriberAgreementsRelativeUrl, subscriberAgreement);

        public async ValueTask<SubscriberAgreement> GetSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            await this.apiFactoryClient.GetContentAsync<SubscriberAgreement>($"{SubscriberAgreementsRelativeUrl}/{subscriberAgreementId}");

        public async ValueTask<List<SubscriberAgreement>> GetAllSubscriberAgreementsAsync()
        {
            var allItems = new List<SubscriberAgreement>();
            string? nextUrl = $"{subscriberAgreementsRelativeOdataUrl}/";

            while (nextUrl is not null)
            {
                OdataResponse<SubscriberAgreement> response =
                    await this.GetOdataResponseAsync<SubscriberAgreement>(nextUrl);

                allItems.AddRange(response.Items);

                nextUrl = response.NextLink is not null
                    ? new Uri(response.NextLink).PathAndQuery.TrimStart('/')
                    : null;
            }

            return allItems;
        }

        public async ValueTask<OdataResponse<SubscriberAgreement>> GetSubscriberAgreementsOdataResponseAsync(
            string relativeOdataUrl) =>
                await this.GetOdataResponseAsync<SubscriberAgreement>(relativeOdataUrl);

        public async ValueTask<SubscriberAgreement> PutSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            await this.apiFactoryClient.PutContentAsync(SubscriberAgreementsRelativeUrl, subscriberAgreement);

        public async ValueTask<SubscriberAgreement> DeleteSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            await this.apiFactoryClient.DeleteContentAsync<SubscriberAgreement>($"{SubscriberAgreementsRelativeUrl}/{subscriberAgreementId}");
    }
}
