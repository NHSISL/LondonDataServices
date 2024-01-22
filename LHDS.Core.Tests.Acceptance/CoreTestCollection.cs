// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Tests.Acceptance.Brokers.DependencyBrokers;
using Xunit;

namespace LHDS.Core.Tests.Acceptance
{
    [CollectionDefinition(nameof(CoreTestCollection))]
    public class CoreTestCollection : ICollectionFixture<DependencyBroker>
    { }
}
