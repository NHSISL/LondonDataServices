// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xunit;

namespace LHDS.Core.Tests.Integration.Decryptions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ReleaseCandidateFactAttribute : FactAttribute
    {
        public ReleaseCandidateFactAttribute()
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
