// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace LHDS.Core.Tests.Integration.TppLandings
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReleaseCandidateFactAttribute : FactAttribute
    {
        public ReleaseCandidateFactAttribute(
            [CallerFilePath] string sourceFile = "",
            [CallerLineNumber] int sourceLine = 0)
            : base(sourceFile, sourceLine)
        {
            var isReleaseCandidate =
                string.Equals(
                    Environment.GetEnvironmentVariable("IS_RELEASE_CANDIDATE"),
                    "true",
                    StringComparison.OrdinalIgnoreCase);

            if (!isReleaseCandidate)
                Skip = "Skipped: IS_RELEASE_CANDIDATE is not set to true.";
        }
    }
}
