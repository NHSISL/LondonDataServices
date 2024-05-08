// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Downloads
{
    public partial class DownloadProcessingService
    {
        private delegate ValueTask<Download> ReturningDownloadFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();

        private async ValueTask<Download> TryCatch(ReturningDownloadFunction returningDownloadFunction)
        {
            try
            {
                return await returningDownloadFunction();
            }
            catch (NullDownloadProcessingException nullDownloadProcessingException)
            {
                throw CreateAndLogValidationException(nullDownloadProcessingException);
            }
            catch (InvalidArgumentDownloadProcessingException invalidArgumentDownloadProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentDownloadProcessingException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadProcessingServiceException =
                    new FailedDownloadProcessingServiceException(
                        message: "Failed Download processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDownloadProcessingServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadProcessingServiceException =
                    new FailedDownloadProcessingServiceException(
                        message: "Failed Download processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDownloadProcessingServiceException);
            }
        }

        private DownloadProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadValidationException = new DownloadProcessingValidationException(
                message: "Download validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }

        private DownloadProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var downloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadProcessingDependencyValidationException);

            return downloadProcessingDependencyValidationException;
        }

        private DownloadProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var downloadProcessingDependencyException =
                new DownloadProcessingDependencyException(
                    message: "Download processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(downloadProcessingDependencyException);

            throw downloadProcessingDependencyException;
        }

        private DownloadProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var downloadProcessingServiceException = new
                DownloadProcessingServiceException(
                    message: "Download processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(downloadProcessingServiceException);

            return downloadProcessingServiceException;
        }
    }
}