// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullDocumentProcessingException nullDocumentException)
            {
                throw CreateAndLogValidationException(nullDocumentException);
            }
        }

        private DocumentProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var documentProcessingValidationExceptionn =
                new DocumentProcessingValidationException(exception);

            this.loggingBroker.LogError(documentProcessingValidationExceptionn);

            return documentProcessingValidationExceptionn;
        }
    }
}
