// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string featuresRelativeUrl = "api/features";

        public async ValueTask<string[]> GetFeaturesAsync() =>
            await this.apiFactoryClient.GetContentAsync<string[]>($"{featuresRelativeUrl}/");
    }
}
