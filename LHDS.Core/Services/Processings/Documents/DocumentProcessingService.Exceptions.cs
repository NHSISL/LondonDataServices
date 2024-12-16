// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Documents
{
    public partial class DocumentProcessingService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<Document> ReturningDocumentProcessingFunction();
        private delegate ValueTask<string> ReturningStringFunction();

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
            catch (InvalidArgumentsDocumentProcessingException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedDocumentProcessingServiceException);
            }
        }

        private async ValueTask<Document> TryCatch(ReturningDocumentProcessingFunction returningDocumentProcessingFunction)
        {
            try
            {
                return await returningDocumentProcessingFunction();
            }
            catch (InvalidArgumentsDocumentProcessingException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDocumentProcessingServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentsDocumentProcessingException exception)
            {
                throw CreateAndLogValidationException(exception);
            }
            catch (NullDocumentProcessingException nullDocumentException)
            {
                throw CreateAndLogValidationException(nullDocumentException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw CreateAndLogValidationException(documentProcessingValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDocumentProcessingServiceException);
            }
        }

        private DocumentProcessingValidationException
            CreateAndLogValidationException(Xeption exception)
        {
            var documentProcessingValidationExceptionn =
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(documentProcessingValidationExceptionn);

            return documentProcessingValidationExceptionn;
        }

        private DocumentProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var documentProcessingDependencyValidationException =
                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentProcessingDependencyValidationException);

            return documentProcessingDependencyValidationException;
        }

        private DocumentProcessingDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var documentProcessingDependencyException =
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentProcessingDependencyException);

            throw documentProcessingDependencyException;
        }

        private DocumentProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var documentProcessingServiceException = new
                DocumentProcessingServiceException(
                message: "Document processing service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(documentProcessingServiceException);

            return documentProcessingServiceException;
        }
    }
}
