using System.Threading.Tasks;
using LHDS.Core.Models.PdsAudits;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public interface IPdsAuditService
    {
        ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit);
    }
}