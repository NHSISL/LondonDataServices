// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;

namespace LHDS.Core.Services.Processings.AddressLoadingAudits
{
    public interface IAddressLoadingAuditProcessingService
    {
        ValueTask<AddressLoadingAudit> AddAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit);
    }
}
