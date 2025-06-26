using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.Core.Services.Foundations.ResolvedAddressAudits
{
    public interface IResolvedAddressAuditService
    {
        ValueTask<ResolvedAddressAudit> AddResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit);
        ValueTask<IQueryable<ResolvedAddressAudit>> RetrieveAllResolvedAddressAuditsAsync();
        ValueTask<ResolvedAddressAudit> RetrieveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId);
        ValueTask<ResolvedAddressAudit> ModifyResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit);
        ValueTask<ResolvedAddressAudit> RemoveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId);
    }
}
