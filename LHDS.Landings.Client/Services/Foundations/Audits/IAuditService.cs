using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits;

namespace LHDS.Landings.Client.Services.Foundations.Audits
{
    public interface IAuditService
    {
        ValueTask<Audit> AddAuditAsync(Audit audit);
        IQueryable<Audit> RetrieveAllAudits();
        ValueTask<Audit> RetrieveAuditByIdAsync(Guid auditId);
        ValueTask<Audit> ModifyAuditAsync(Audit audit);
    }
}