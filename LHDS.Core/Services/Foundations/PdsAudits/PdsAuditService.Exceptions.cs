using System.Threading.Tasks;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private delegate ValueTask<PdsAudit> ReturningPdsAuditFunction();

        private async ValueTask<PdsAudit> TryCatch(ReturningPdsAuditFunction returningPdsAuditFunction)
        {
            try
            {
                return await returningPdsAuditFunction();
            }
            catch (NullPdsAuditException nullPdsAuditException)
            {
                throw CreateAndLogValidationException(nullPdsAuditException);
            }
        }

        private PdsAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsAuditValidationException =
                new PdsAuditValidationException(exception);

            this.loggingBroker.LogError(pdsAuditValidationException);

            return pdsAuditValidationException;
        }
    }
}