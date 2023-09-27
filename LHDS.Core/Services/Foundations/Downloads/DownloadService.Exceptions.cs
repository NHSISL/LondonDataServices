// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private delegate ValueTask<List<Document>> ReturningDownloadsFunction();
        private delegate ValueTask<Document> ReturningDownloadFunction();

        private async ValueTask<Document> TryCatch(ReturningDownloadFunction returningDownloadFunction)
        {
            try
            {
                return await returningDownloadFunction();
            }
            catch (NullDownloadException nullDownloadException)
            {
                throw CreateAndLogValidationException(nullDownloadException);
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
                    new FailedDownloadServiceException(exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private async ValueTask<List<Document>> TryCatch(ReturningDownloadsFunction returningDownloadsFunction)
        {
            try
            {
                return await returningDownloadsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDownloadStorageException =
                    new FailedDownloadStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedDownloadStorageException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadServiceException(exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private DownloadValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadValidationException = new DownloadValidationException(exception);
            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }

        private DownloadDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var downloadDependencyException = new DownloadDependencyException(
                message: "Download dependency error occurred, contact support.", 
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
                message: "Download dependency error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(downloadDependencyException);

            return downloadDependencyException;
        }

        private DownloadServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var downloadServiceException = new DownloadServiceException(
                message: "Download service error occurred, contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(downloadServiceException);

            return downloadServiceException;
        }
    }
}