// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.Models.Foundations.OptOuts
{
    public class OptOutIdentifier
    {
        public string UniqueReference { get; set; }
        public string NhsNumber { get; set; }
        public string Status { get; set; } = "Unknown";
    }
}
