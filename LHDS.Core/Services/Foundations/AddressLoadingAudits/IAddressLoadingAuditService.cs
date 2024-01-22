using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public interface IAddressLoadingAuditService
    {
        ValueTask<AddressLoadingAudit> AddAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit);
        IQueryable<AddressLoadingAudit> RetrieveAllAddressLoadingAudits();
        ValueTask<AddressLoadingAudit> RetrieveAddressLoadingAuditByIdAsync(Guid addressLoadingAuditId);
        ValueTask<AddressLoadingAudit> ModifyAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit);
        ValueTask<AddressLoadingAudit> RemoveAddressLoadingAuditByIdAsync(Guid addressLoadingAuditId);
    }
}