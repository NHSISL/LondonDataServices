using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string SubscriberCredentialsRelativeUrl = "api/subscriberCredentials";

        public async ValueTask<SubscriberCredential> PostSubscriberCredentialAsync(SubscriberCredential subscriberCredential) =>
            await this.apiFactoryClient.PostContentAsync(SubscriberCredentialsRelativeUrl, subscriberCredential);

        public async ValueTask<SubscriberCredential> GetSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            await this.apiFactoryClient.GetContentAsync<SubscriberCredential>($"{SubscriberCredentialsRelativeUrl}/{subscriberCredentialId}");

        public async ValueTask<List<SubscriberCredential>> GetAllSubscriberCredentialsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<SubscriberCredential>>($"{SubscriberCredentialsRelativeUrl}/");

        public async ValueTask<SubscriberCredential> PutSubscriberCredentialAsync(SubscriberCredential subscriberCredential) =>
            await this.apiFactoryClient.PutContentAsync(SubscriberCredentialsRelativeUrl, subscriberCredential);

        public async ValueTask<SubscriberCredential> DeleteSubscriberCredentialByIdAsync(Guid subscriberCredentialId) =>
            await this.apiFactoryClient.DeleteContentAsync<SubscriberCredential>($"{SubscriberCredentialsRelativeUrl}/{subscriberCredentialId}");
    }
}
