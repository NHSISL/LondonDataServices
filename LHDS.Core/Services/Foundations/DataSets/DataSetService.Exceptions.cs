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
                throw CreateAndLogValidationException(nullDataSetException);
            }
            catch (InvalidDataSetException invalidDataSetException)
            {
                throw CreateAndLogValidationException(invalidDataSetException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetStorageException);
            }
            catch (NotFoundDataSetException notFoundDataSetException)
            {
                throw CreateAndLogValidationException(notFoundDataSetException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetException =
                    new AlreadyExistsDataSetException(
                        message: "DataSet with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataSetException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetReferenceException =
                    new InvalidDataSetReferenceException(
                        message: "Invalid dataSet reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataSetReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataSetException =
                    new LockedDataSetException(
                        message: "Locked dataSet record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedDataSetException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedDataSetStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetServiceException =
                    new FailedDataSetServiceException(
                        message: "Failed dataSet service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetServiceException);
            }
        }

        private async ValueTask<IQueryable<DataSet>> TryCatch(ReturningDataSetsFunction returningDataSetsFunction)
        {
            try
            {
                return await  returningDataSetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetServiceException =
                    new FailedDataSetServiceException(
                        message: "Failed dataSet service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetServiceException);
            }
        }

        private DataSetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetValidationException);

            return dataSetValidationException;
        }

        private DataSetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private DataSetDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetDependencyValidationException);

            return dataSetDependencyValidationException;
        }

        private DataSetDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var dataSetDependencyException =
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private DataSetServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataSetServiceException =
                new DataSetServiceException(
                    message: "DataSet service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetServiceException);

            return dataSetServiceException;
        }
    }
}