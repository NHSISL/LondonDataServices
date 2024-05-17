// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace LHDS.Core.Tests.Integration.Terminology
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "LHDS.Core.Tests.Integration.Terminology.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "LHDS.Core.Tests.Integration.Terminology")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
