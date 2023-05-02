using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private void ValidatePdsAuditOnAdd(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);
        }

        private static void ValidatePdsAuditIsNotNull(PdsAudit pdsAudit)
        {
            if (pdsAudit is null)
            {
                throw new NullPdsAuditException();
            }
        }
    }
}