using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public interface IAddressExtractionAuditService
    {
        ValueTask<AddressExtractionAudit> AddAddressExtractionAuditAsync(AddressExtractionAudit addressExtractionAudit);
        IQueryable<AddressExtractionAudit> RetrieveAllAddressExtractionAudits();
    }
}