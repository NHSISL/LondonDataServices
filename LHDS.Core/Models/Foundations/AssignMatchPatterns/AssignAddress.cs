// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.AssignMatchPatterns
{
    public class AssignMatchPattern : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
