// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<AddressExtractionAudit> AddressExtractionAudits { get; set; }

        public async ValueTask<AddressExtractionAudit> InsertAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit) => await InsertAsync(addressExtractionAudit);

        public IQueryable<AddressExtractionAudit> SelectAllAddressExtractionAudits() =>
            ReadAll<AddressExtractionAudit>();

        public async ValueTask<AddressExtractionAudit> SelectAddressExtractionAuditByIdAsync(
            Guid addressExtractionAuditId) => await ReadAsync<AddressExtractionAudit>(addressExtractionAuditId);

        public async ValueTask<AddressExtractionAudit> UpdateAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit) => await UpdateAsync(addressExtractionAudit);

        public async ValueTask<AddressExtractionAudit> DeleteAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit) => await DeleteAsync(addressExtractionAudit);
    }
}
