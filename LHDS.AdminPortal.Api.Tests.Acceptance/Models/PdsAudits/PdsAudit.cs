// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits
{
    public class PdsAudit
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string FileName { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
