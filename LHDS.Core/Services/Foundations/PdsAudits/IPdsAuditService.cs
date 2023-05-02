using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.PdsAudits;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public interface IPdsAuditService
    {
        ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit);
        IQueryable<PdsAudit> RetrieveAllPdsAudits();
        ValueTask<PdsAudit> RetrievePdsAuditByIdAsync(Guid pdsAuditId);
    }
}