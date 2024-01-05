// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string terminologyArtifactDetailsRelativeUrl = "api/terminologyArtifactDetails";

        public async ValueTask RetrieveArtifactDetailsAsync() =>
             await this.apiFactoryClient.PostContentAsync(terminologyArtifactDetailsRelativeUrl, null);
    }
}
