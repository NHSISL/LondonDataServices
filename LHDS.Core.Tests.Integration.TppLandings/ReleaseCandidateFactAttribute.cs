// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "LHDS.Core.Tests.Integration.TppLandings.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "LHDS.Core.Tests.Integration.TppLandings")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
