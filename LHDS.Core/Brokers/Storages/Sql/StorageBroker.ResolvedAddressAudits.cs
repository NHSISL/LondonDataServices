using System.Linq;
using System.Threading.Tasks;
using System;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker 
    {
        public DbSet<ResolvedAddressAudit> ResolvedAddressAudits { get; set; }

        public async ValueTask<ResolvedAddressAudit> InsertResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit) =>
            await InsertAsync(resolvedAddressAudit);

        public async ValueTask<IQueryable<ResolvedAddressAudit>> SelectAllResolvedAddressAuditsAsync() =>
            await SelectAllAsync<ResolvedAddressAudit>();

        public async ValueTask<ResolvedAddressAudit> SelectResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId) =>
            await SelectAsync<ResolvedAddressAudit>(resolvedAddressAuditId);

        public async ValueTask<ResolvedAddressAudit> UpdateResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit) =>
            await UpdateAsync(resolvedAddressAudit);

        public async ValueTask<ResolvedAddressAudit> DeleteResolvedAddressAuditAsync(ResolvedAddressAudit resolvedAddressAudit) =>
            await DeleteAsync(resolvedAddressAudit);
    }
}
