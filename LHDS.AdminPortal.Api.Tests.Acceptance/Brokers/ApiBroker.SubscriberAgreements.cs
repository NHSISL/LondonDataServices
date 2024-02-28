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
        private const string subscriberAgreementsRelativeUrl = "api/subscriberAgreements";
        private const string subscriberAgreementsRelativeOdataUrl = "odata/subscriberAgreements";

        public async ValueTask<SubscriberAgreement> PostSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            await this.apiFactoryClient.PostContentAsync(subscriberAgreementsRelativeUrl, subscriberAgreement);

        public async ValueTask<List<SubscriberAgreement>> GetAllSubscriberAgreementsAsync()
        {
            OdataResponse<SubscriberAgreement> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<SubscriberAgreement>>
                    ($"{subscriberAgreementsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<List<SubscriberAgreement>> FindSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
              await this.apiFactoryClient.GetContentAsync<List<SubscriberAgreement>>(
                  $"{subscriberAgreementsRelativeUrl}/?$filter=Id eq {subscriberAgreementId}");

        public async ValueTask<SubscriberAgreement> GetSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            await this.apiFactoryClient.GetContentAsync<SubscriberAgreement>($"{subscriberAgreementsRelativeUrl}/{subscriberAgreementId}");

        public async ValueTask<SubscriberAgreement> PutSubscriberAgreementAsync(SubscriberAgreement subscriberAgreementId) =>
            await this.apiFactoryClient.PutContentAsync(subscriberAgreementsRelativeUrl, subscriberAgreementId);

        public async ValueTask<SubscriberAgreement> DeleteSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            await this.apiFactoryClient.DeleteContentAsync<SubscriberAgreement>
                ($"{subscriberAgreementsRelativeUrl}/{subscriberAgreementId}");
    }
}
