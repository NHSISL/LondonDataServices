// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public interface IPdsAuditService
    {
        ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit);
        IQueryable<PdsAudit> RetrieveAllPdsAudits();
        ValueTask<PdsAudit> RetrievePdsAuditByIdAsync(Guid pdsAuditId);
        ValueTask<PdsAudit> ModifyPdsAuditAsync(PdsAudit pdsAudit);
        ValueTask<PdsAudit> RemovePdsAuditByIdAsync(Guid pdsAuditId);
    }
}