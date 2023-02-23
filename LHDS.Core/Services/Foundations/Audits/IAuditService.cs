using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public interface IAuditService
    {
        ValueTask<Audit> AddAuditAsync(Audit audit);
        IQueryable<Audit> RetrieveAllAudits();
    }
}