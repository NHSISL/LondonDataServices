using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Audits.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Audits
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