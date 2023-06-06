// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationService
    {
        private delegate ValueTask<PdsAudit> ReturningPdsAuditFunction();

        private async ValueTask<PdsAudit> TryCatch(ReturningPdsAuditFunction returningPdsAuditFunction)
        {
            try
            {
                return await returningPdsAuditFunction();
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw CreateAndLogValidationException(invalidArgumentPdsException);
            }
        }

        private PdsValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsValidationException =
                new PdsValidationException(exception);

            this.loggingBroker.LogError(pdsValidationException);

            return pdsValidationException;
        }
    }
}
