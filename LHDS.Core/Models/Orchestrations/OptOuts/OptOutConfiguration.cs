// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Orchestrations.OptOuts
{
    public class OptOutConfiguration
    {
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public int ExpiredAfterDays { get; set; }
        public bool OptOutFileHasHeader { get; set; }
        public bool OptOutFileRequireTrailingComma { get; set; }
        public string To { get; set; }
        public string WorkflowId { get; set; }
    }
}
