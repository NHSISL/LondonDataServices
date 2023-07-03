// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(CoreTestCollection))]
    public class CoreTestCollection : ICollectionFixture<DependencyBroker>
    { }
}
