// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
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
            catch (RequestFailedException requestFailedException)
            {
                var failedRequestException = new FailedDocumentRequestException(requestFailedException);
                throw CreateAndLogDependencyException(failedRequestException);
            }
            catch (Exception exception)
            {
                var failedDocumentServiceException =
                   new FailedDocumentServiceException(exception);

                throw CreateAndLogServiceException(failedDocumentServiceException);
            }
        }

        private DocumentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var documentValidationExceptionn =
                new DocumentValidationException(exception);

            this.loggingBroker.LogError(documentValidationExceptionn);

            return documentValidationExceptionn;
        }
        private Exception CreateAndLogServiceException(Xeption exception)
        {
            var documentServiceException = new DocumentServiceException(exception);
            this.loggingBroker.LogError(documentServiceException);

            return documentServiceException;
        }

        private DocumentDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var documentDependencyException = new DocumentDependencyException(exception);
            this.loggingBroker.LogError(documentDependencyException);

            return documentDependencyException;
        }
    }
}
