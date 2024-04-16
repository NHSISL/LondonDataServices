// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<Audit> InsertAuditAsync(Audit audit);
        IQueryable<Audit> SelectAllAudits();
        ValueTask<Audit> SelectAuditByIdAsync(Guid auditId);
        ValueTask<Audit> UpdateAuditAsync(Audit audit);
        ValueTask<Audit> DeleteAuditAsync(Audit audit);
    }
}
