// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;
using Xunit.Sdk;

namespace LHDS.Core.Tests.Integration.OptOuts
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer(
        typeName: "LHDS.Core.Tests.Integration.OptOuts.ReleaseCandidateTestCaseDiscoverer",
        assemblyName: "LHDS.Core.Tests.Integration.OptOuts")]
    public class ReleaseCandidateFactAttribute : FactAttribute { }
}
