// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Downloads
{
    public partial class DownloadProcessingService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullDownloadProcessingException nullDownloadProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDownloadProcessingException);
            }
            catch (InvalidArgumentDownloadProcessingException invalidArgumentDownloadProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDownloadProcessingException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadProcessingServiceException =
                    new FailedDownloadProcessingServiceException(
                        message: "Failed Download processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDownloadProcessingServiceException);
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
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadProcessingServiceException =
                    new FailedDownloadProcessingServiceException(
                        message: "Failed Download processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDownloadProcessingServiceException);
            }
        }

        private async ValueTask<DownloadProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var downloadValidationException = new DownloadProcessingValidationException(
                message: "Download validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(downloadValidationException);

            return downloadValidationException;
        }

        private async ValueTask<DownloadProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var downloadProcessingDependencyValidationException =
                new DownloadProcessingDependencyValidationException(
                    message: "Download processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(downloadProcessingDependencyValidationException);

            return downloadProcessingDependencyValidationException;
        }

        private async ValueTask<DownloadProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var downloadProcessingDependencyException =
                new DownloadProcessingDependencyException(
                    message: "Download processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(downloadProcessingDependencyException);

            return downloadProcessingDependencyException;
        }

        private async ValueTask<DownloadProcessingServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var downloadProcessingServiceException = new
                DownloadProcessingServiceException(
                    message: "Download processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(downloadProcessingServiceException);

            return downloadProcessingServiceException;
        }
    }
}