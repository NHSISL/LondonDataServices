// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Tpp.Exceptions;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService
    {
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (NullTppDocumentException nullTppDocumentException)
            {
                throw CreateAndLogValidationException(nullTppDocumentException);
            }
            catch (InvalidArgumentException invalidArgumentException)
            {
                throw CreateAndLogValidationException(invalidArgumentException);
            }
            catch (TppDocumentValidationException tppDocumentValidationException)
            {
                throw CreateAndLogValidationException(tppDocumentValidationException);
            }
        }

        private TppDocumentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tppDocumentValidationExceptionn =
                new TppDocumentValidationException(
                    message: "Tpp Document validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(tppDocumentValidationExceptionn);

            return tppDocumentValidationExceptionn;
        }
    }
}
