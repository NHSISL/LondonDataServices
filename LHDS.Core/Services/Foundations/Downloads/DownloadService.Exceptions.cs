// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullDownloadException nullDownloadException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDownloadException);
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialException);
            }
            catch (NullDocumentException nullDocumentException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDocumentException);
            }
            catch (InvalidDownloadException invalidDownloadException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDownloadException);
            }
            catch (NotFoundDownloadException notFoundDownloadException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDownloadException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadServiceException(
                        message: "Failed download service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDownloadServiceException);
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

                throw await CreateAndLogServiceExceptionAsync(failedDownloadServiceException);
            }
        }

        private async ValueTask<DownloadValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var downloadValidationException = new DownloadValidationException(
                message: "Download validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(downloadValidationException);

            return downloadValidationException;
        }

        private async ValueTask<DownloadServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var downloadServiceException = new DownloadServiceException(
                message: "Download service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(downloadServiceException);

            return downloadServiceException;
        }
    }
}