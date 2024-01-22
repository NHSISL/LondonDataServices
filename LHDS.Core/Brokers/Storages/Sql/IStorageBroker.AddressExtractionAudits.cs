// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<AddressExtractionAudit> InsertAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit);

        IQueryable<AddressExtractionAudit> SelectAllAddressExtractionAudits();
        ValueTask<AddressExtractionAudit> SelectAddressExtractionAuditByIdAsync(Guid addressExtractionAuditId);

        ValueTask<AddressExtractionAudit> UpdateAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit);

        ValueTask<AddressExtractionAudit> DeleteAddressExtractionAuditAsync(
            AddressExtractionAudit addressExtractionAudit);
    }
}
