using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public interface IAuditService
    {
        ValueTask<Audit> AddAuditAsync(Audit audit);
        IQueryable<Audit> RetrieveAllAudits();
        ValueTask<Audit> RetrieveAuditByIdAsync(Guid auditId);
    }
}