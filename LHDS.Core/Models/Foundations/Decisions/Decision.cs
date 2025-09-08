// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;

namespace LHDS.Core.Models.Foundations.Decisions
{
    public class Decision
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DecisionTypeId { get; set; }
        public string DecisionChoice { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public string? ResponsiblePersonGivenName { get; set; }
        public string? ResponsiblePersonSurname { get; set; }
        public string? ResponsiblePersonRelationship { get; set; }
        public DecisionType DecisionType { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
    }
}
