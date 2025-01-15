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
                throw await CreateAndLogValidationExceptionAsync(nullDocumentException);
            }
            catch (InvalidArgumentsDocumentProcessingException exception)
            {
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedDocumentProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDocumentProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(exception);
            }
            catch (NullDocumentProcessingException nullDocumentException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDocumentException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw await CreateAndLogValidationExceptionAsync(documentProcessingValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (Exception exception)
            {
                var failedDocumentProcessingServiceException =
                    new FailedDocumentProcessingServiceException(
                        message: "Failed document processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDocumentProcessingServiceException);
            }
        }

        private async ValueTask<DocumentProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var documentProcessingValidationExceptionn =
                new DocumentProcessingValidationException(
                    message: "Document processing validation errors occured, please try again",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(documentProcessingValidationExceptionn);

            return documentProcessingValidationExceptionn;
        }

        private async ValueTask<DocumentProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var documentProcessingDependencyValidationException =
                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(documentProcessingDependencyValidationException);

            return documentProcessingDependencyValidationException;
        }

        private async ValueTask<DocumentProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var documentProcessingDependencyException =
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(documentProcessingDependencyException);

            return documentProcessingDependencyException;
        }

        private async ValueTask<DocumentProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var documentProcessingServiceException = new
                DocumentProcessingServiceException(
                message: "Document processing service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(documentProcessingServiceException);

            return documentProcessingServiceException;
        }
    }
}
