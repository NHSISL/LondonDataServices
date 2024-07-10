// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace LHDS.Core.Tests.Integration.EmisLandings
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "LHDS.Core.Tests.Integration.EmisLandings.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "LHDS.Core.Tests.Integration.EmisLandings")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
