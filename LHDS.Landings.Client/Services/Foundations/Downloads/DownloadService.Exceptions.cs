using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Landings.Client.Models.Downloads;
using LHDS.Landings.Client.Models.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.Downloads
{
    public partial class DownloadService
    {
        private delegate ValueTask<Download> ReturningDownloadFunction();
        private delegate IQueryable<Download> ReturningDownloadsFunction();

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
            catch (InvalidDownloadException invalidDownloadException)
            {
                throw CreateAndLogValidationException(invalidDownloadException);
            }
            catch (SqlException sqlException)
            {
                var failedDownloadStorageException =
                    new FailedDownloadStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDownloadStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDownloadException =
                    new AlreadyExistsDownloadException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDownloadException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDownloadReferenceException =
                    new InvalidDownloadReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDownloadReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDownloadStorageException =
                    new FailedDownloadStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedDownloadStorageException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadServiceException(exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private IQueryable<Download> TryCatch(ReturningDownloadsFunction returningDownloadsFunction)
        {
            try
            {
                return returningDownloadsFunction();
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
            var downloadValidationException =
                new DownloadValidationException(exception);

            this.loggingBroker.LogError(downloadValidationException);

            return downloadValidationException;
        }

        private DownloadDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var downloadDependencyException = new DownloadDependencyException(exception);
            this.loggingBroker.LogCritical(downloadDependencyException);

            return downloadDependencyException;
        }

        private DownloadDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var downloadDependencyValidationException =
                new DownloadDependencyValidationException(exception);

            this.loggingBroker.LogError(downloadDependencyValidationException);

            return downloadDependencyValidationException;
        }

        private DownloadDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var downloadDependencyException = new DownloadDependencyException(exception);
            this.loggingBroker.LogError(downloadDependencyException);

            return downloadDependencyException;
        }

        private DownloadServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var downloadServiceException = new DownloadServiceException(exception);
            this.loggingBroker.LogError(downloadServiceException);

            return downloadServiceException;
        }
    }
}