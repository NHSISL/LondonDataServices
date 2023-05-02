// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.PdsAudits
{
    public class PdsAudit : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
