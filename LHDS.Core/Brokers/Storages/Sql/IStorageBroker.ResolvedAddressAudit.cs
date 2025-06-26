using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<ResolvedAddressAudit> InsertResolvedAddressAuditAsync(ResolvedAddressAudit ResolvedAddressAudit);
        ValueTask<IQueryable<ResolvedAddressAudit>> SelectAllResolvedAddressAuditsAsync();
        ValueTask<ResolvedAddressAudit> SelectResolvedAddressAuditByIdAsync(Guid ResolvedAddressAuditId);
        ValueTask<ResolvedAddressAudit> UpdateResolvedAddressAuditAsync(ResolvedAddressAudit ResolvedAddressAudit);
        ValueTask<ResolvedAddressAudit> DeleteResolvedAddressAuditAsync(ResolvedAddressAudit ResolvedAddressAudit);
    }
}
