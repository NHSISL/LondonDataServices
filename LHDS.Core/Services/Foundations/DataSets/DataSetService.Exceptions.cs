// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private delegate ValueTask<DataSet> ReturningDataSetFunction();
        private delegate ValueTask<IQueryable<DataSet>> ReturningDataSetsFunction();

        private async ValueTask<DataSet> TryCatch(ReturningDataSetFunction returningDataSetFunction)
        {
            try
            {
                return await returningDataSetFunction();
            }
            catch (NullDataSetException nullDataSetException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDataSetException);
            }
            catch (InvalidDataSetException invalidDataSetException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDataSetException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataSetStorageException);
            }
            catch (NotFoundDataSetException notFoundDataSetException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDataSetException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetException =
                    new AlreadyExistsDataSetException(
                        message: "DataSet with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsDataSetException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetReferenceException =
                    new InvalidDataSetReferenceException(
                        message: "Invalid dataSet reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDataSetReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataSetException =
                    new LockedDataSetException(
                        message: "Locked dataSet record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedDataSetException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedDataSetStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetServiceException =
                    new FailedDataSetServiceException(
                        message: "Failed dataSet service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetServiceException);
            }
        }

        private async ValueTask<IQueryable<DataSet>> TryCatch(ReturningDataSetsFunction returningDataSetsFunction)
        {
            try
            {
                return await returningDataSetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataSetStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetServiceException =
                    new FailedDataSetServiceException(
                        message: "Failed dataSet service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetServiceException);
            }
        }

        private async ValueTask<DataSetValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var dataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetValidationException);

            return dataSetValidationException;
        }

        private async ValueTask<DataSetDependencyException> CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var dataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private async ValueTask<DataSetDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetDependencyValidationException);

            return dataSetDependencyValidationException;
        }

        private async ValueTask<DataSetDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var dataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private async ValueTask<DataSetServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var dataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetServiceException);

            return dataSetServiceException;
        }
    }
}