using LHDS.Core.Models.Audits;
using LHDS.Core.Models.Audits.Exceptions;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService
    {
        private void ValidateAuditOnAdd(Audit audit)
        {
            ValidateAuditIsNotNull(audit);
        }

        private static void ValidateAuditIsNotNull(Audit audit)
        {
            if (audit is null)
            {
                throw new NullAuditException();
            }
        }
    }
}