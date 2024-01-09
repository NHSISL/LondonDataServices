// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string optOutsWorkflowRelativeUrl = "api/optouts";

        public async ValueTask<MeshMessage> PushExpiredOptOutsToMeshForRenewalAsync() =>
            await this.apiFactoryClient.PostContentAsync<OptOut>(optOutsRelativeUrl);
    }
}
