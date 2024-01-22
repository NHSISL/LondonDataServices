// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public interface IAddressExtractionAuditService
    {
        ValueTask<AddressExtractionAudit> AddAddressExtractionAuditAsync(AddressExtractionAudit addressExtractionAudit);
        IQueryable<AddressExtractionAudit> RetrieveAllAddressExtractionAudits();
        ValueTask<AddressExtractionAudit> RetrieveAddressExtractionAuditByIdAsync(Guid addressExtractionAuditId);
        ValueTask<AddressExtractionAudit> ModifyAddressExtractionAuditAsync(AddressExtractionAudit addressExtractionAudit);
        ValueTask<AddressExtractionAudit> RemoveAddressExtractionAuditByIdAsync(Guid addressExtractionAuditId);
    }
}