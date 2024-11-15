// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Tests.Acceptance.Brokers.DependencyBrokers;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Acceptance
{
    [CollectionDefinition(nameof(CoreTestCollection))]
    public class CoreTestCollection : ICollectionFixture<DependencyBroker>
    { }
}
