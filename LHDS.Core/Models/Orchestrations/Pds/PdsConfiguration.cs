// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Models.Orchestrations.Pds
{
    public class PdsConfiguration
    {
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public bool PdsFileHasHeader { get; set; }
        public bool PdsFileRequireTrailingComma { get; set; }
        public string To { get; set; }
        public string WorkflowId { get; set; }
    }
}
