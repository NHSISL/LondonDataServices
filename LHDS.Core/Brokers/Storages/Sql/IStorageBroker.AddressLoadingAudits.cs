// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<AddressLoadingAudit> InsertAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit);

        IQueryable<AddressLoadingAudit> SelectAllAddressLoadingAudits();
        ValueTask<AddressLoadingAudit> SelectAddressLoadingAuditByIdAsync(Guid addressLoadingAuditId);

        ValueTask<AddressLoadingAudit> UpdateAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit);

        ValueTask<AddressLoadingAudit> DeleteAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit);
    }
}
