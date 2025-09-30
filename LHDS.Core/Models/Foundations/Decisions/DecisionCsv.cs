// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace LHDS.Core.Models.Foundations.Decisions
{
    public class DecisionCsv
    {
        public Guid DecisionId { get; set; }
        public string NhsHash { get; set; } = string.Empty;
        public string PatientInstructionCategory { get; set; } = string.Empty;
        public string PatientInstructionState { get; set; } = string.Empty;
        public DateTimeOffset InstructionDate { get; set; }
    }
}
