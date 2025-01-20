// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<PdsAudit> PdsAudits { get; set; }

        public async ValueTask<PdsAudit> InsertPdsAuditAsync(PdsAudit pdsAudit) =>
            await InsertAsync(pdsAudit);

        public async ValueTask<IQueryable<PdsAudit>> SelectAllPdsAuditsAsync() =>
            await SelectAllAsync<PdsAudit>();

        public async ValueTask<PdsAudit> SelectPdsAuditByIdAsync(Guid pdsAuditId) =>
            await SelectAsync<PdsAudit>(pdsAuditId);

        public async ValueTask<PdsAudit> UpdatePdsAuditAsync(PdsAudit pdsAudit) =>
            await UpdateAsync(pdsAudit);

        public async ValueTask<PdsAudit> DeletePdsAuditAsync(PdsAudit pdsAudit) =>
            await DeleteAsync(pdsAudit);
    }
}
