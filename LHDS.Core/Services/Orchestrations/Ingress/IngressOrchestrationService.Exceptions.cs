// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentIngressOrchestrationException invalidArgumentIngressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngressOrchestrationException);
            }
            catch (NotFoundIngressOrchestrationException notFoundIngressOrchestrationException)
            {
                throw CreateAndLogValidationException(notFoundIngressOrchestrationException);
            }
        }

        private IngressOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var emisLandingOrchestrationValidationException =
                new IngressOrchestrationValidationException(
                    message: "Ingress orchestration validation errors occurred, please try again.",
                    exception);

            this.loggingBroker.LogError(emisLandingOrchestrationValidationException);

            return emisLandingOrchestrationValidationException;
        }
    }
}
