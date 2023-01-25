// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using NEL.Premises.Api.Models.Documents.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DocumentService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullDocumentException nullDocumentException)
            {
                throw CreateAndLogValidationException(nullDocumentException);
            }
            catch (InvalidDocumentException invalidDocumentException)
            {
                throw CreateAndLogValidationException(invalidDocumentException);
            }
        }

        private DocumentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var documentValidationExceptionn =
                new DocumentValidationException(exception);

            this.loggingBroker.LogError(documentValidationExceptionn);

            return documentValidationExceptionn;
        }
    }
}
