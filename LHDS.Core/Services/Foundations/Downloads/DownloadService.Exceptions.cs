// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask<Download> ReturningDownloadFunction();

        private async ValueTask<Download> TryCatch(ReturningDownloadFunction returningDownloadFunction)
        {
            try
            {
                return await returningDownloadFunction();
            }
            catch (NullDownloadException nullDownloadException)
            {
                throw CreateAndLogValidationException(nullDownloadException);
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialException);
            }
            catch (NullDocumentException nullDocumentException)
            {
                throw CreateAndLogValidationException(nullDocumentException);
            }
            catch (InvalidDownloadException invalidDownloadException)
            {
                throw CreateAndLogValidationException(invalidDownloadException);
            }
            catch (NotFoundDownloadException notFoundDownloadException)
            {
                throw CreateAndLogValidationException(notFoundDownloadException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadServiceException(
                        message: "Failed download service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadServiceException(
                        message: "Failed download service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private DownloadValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadValidationException = new DownloadValidationException(
                message: "Download validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }

        private DownloadDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var downloadDependencyException = new DownloadDependencyException(
                message: "Download dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogCritical(downloadDependencyException);

            return downloadDependencyException;
        }

        private DownloadDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var downloadDependencyValidationException =
                new DownloadDependencyValidationException(
                    message: "Download dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(downloadDependencyValidationException);

            return downloadDependencyValidationException;
        }

        private DownloadDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var downloadDependencyException = new DownloadDependencyException(
                message: "Download dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(downloadDependencyException);

            return downloadDependencyException;
        }

        private DownloadServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var downloadServiceException = new DownloadServiceException(
                message: "Download service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(downloadServiceException);

            return downloadServiceException;
        }
    }
}