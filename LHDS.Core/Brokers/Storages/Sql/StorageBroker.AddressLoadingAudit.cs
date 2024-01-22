// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<AddressLoadingAudit> AddressLoadingAudits { get; set; }

        public async ValueTask<AddressLoadingAudit> InsertAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit) => await InsertAsync(addressLoadingAudit);

        public IQueryable<AddressLoadingAudit> SelectAllAddressLoadingAudits() =>
            ReadAll<AddressLoadingAudit>();

        public async ValueTask<AddressLoadingAudit> SelectAddressLoadingAuditByIdAsync(
            Guid addressLoadingAuditId) => await ReadAsync<AddressLoadingAudit>(addressLoadingAuditId);

        public async ValueTask<AddressLoadingAudit> UpdateAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit) => await UpdateAsync(addressLoadingAudit);

        public async ValueTask<AddressLoadingAudit> DeleteAddressLoadingAuditAsync(
            AddressLoadingAudit addressLoadingAudit) => await DeleteAsync(addressLoadingAudit);
    }
}
