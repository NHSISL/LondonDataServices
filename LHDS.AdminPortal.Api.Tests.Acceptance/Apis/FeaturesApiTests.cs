// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.AdminPortal.Api.Tests.Acceptance.Brokers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Features
{
    [Collection(nameof(ApiTestCollection))]
    public partial class FeaturesApiTests
    {
        private readonly ApiBroker apiBroker;

        public FeaturesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;
    }
}