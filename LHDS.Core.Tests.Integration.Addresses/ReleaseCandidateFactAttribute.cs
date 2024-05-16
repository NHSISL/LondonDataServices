// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace LHDS.Core.Tests.Integration.Addresses
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "LHDS.Core.Tests.Integration.Addresses.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "LHDS.Core.Tests.Integration.Addresses")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
