// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Attributes;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses
{
    public enum MatchAlgorithmEnum
    {
        [Parameter(Value = "Exact")]
        Exact,

        [Parameter(Value = "Best match")]
        BestMatch,

        [Parameter(Value = "Human")]
        Human
    }
}
