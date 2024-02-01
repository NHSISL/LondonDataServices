// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Attributes;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses
{
    public enum MatchAlgorithmEnum
    {
        [Parameter(Value = "Human")]
        Human = 0,

        [Parameter(Value = "Exact")]
        Exact = 1,

        [Parameter(Value = "Best match")]
        BestMatch = 2,
    }
}
