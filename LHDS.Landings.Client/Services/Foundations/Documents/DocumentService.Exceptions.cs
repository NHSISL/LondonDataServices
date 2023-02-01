// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Azure;
using EFxceptions.Models.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Documents;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using NEL.Premises.Api.Models.Documents.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.Documents
{
    public partial class DocumentService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<Document> ReturningDocumentFunction();

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
            catch (SqlException sqlException)
            {
                var failedDocumentStorageException =
                    new FailedDocumentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDocumentStorageException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedRequestException = new FailedDocumentRequestException(requestFailedException);
                throw CreateAndLogDependencyException(failedRequestException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDocumentException =
                    new AlreadyExistsDocumentException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDocumentException);
            }
            catch (Exception exception)
            {
                var failedDocumentServiceException =
                   new FailedDocumentServiceException(exception);

                throw CreateAndLogServiceException(failedDocumentServiceException);
            }
        }

        private async ValueTask<Document> TryCatch(ReturningDocumentFunction returningDocumentFunction)
        {
            try
            {
                return await returningDocumentFunction();
            }
            catch (InvalidDocumentException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedRequestException = new FailedDocumentRequestException(requestFailedException);
                throw CreateAndLogDependencyException(failedRequestException);
            }
            catch (Exception exception)
            {
                var failedDocumentBlobServiceException =
                   new FailedDocumentServiceException(exception);

                throw CreateAndLogServiceException(failedDocumentBlobServiceException);
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

        private DocumentDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var documentDependencyValidationException =
                new DocumentDependencyValidationException(exception);

            this.loggingBroker.LogError(documentDependencyValidationException);

            return documentDependencyValidationException;
        }

        private DocumentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var documentDependencyException = new DocumentDependencyException(exception);
            this.loggingBroker.LogCritical(documentDependencyException);

            return documentDependencyException;
        }
    }
}
