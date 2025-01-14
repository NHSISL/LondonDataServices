// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private delegate ValueTask<DataSetSpecification> ReturningDataSetSpecificationFunction();
        private delegate ValueTask<IQueryable<DataSetSpecification>> ReturningDataSetSpecificationsFunction();

        private async ValueTask<DataSetSpecification> TryCatch(
            ReturningDataSetSpecificationFunction returningDataSetSpecificationFunction)
        {
            try
            {
                return await returningDataSetSpecificationFunction();
            }
            catch (NullDataSetSpecificationException nullDataSetSpecificationException)
            {
                throw CreateAndLogValidationException(nullDataSetSpecificationException);
            }
            catch (InvalidDataSetSpecificationException invalidDataSetSpecificationException)
            {
                throw CreateAndLogValidationException(invalidDataSetSpecificationException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetSpecificationStorageException =
                    new FailedDataSetSpecificationStorageException(
                        message: "Failed dataSetSpecification storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetSpecificationStorageException);
            }
            catch (NotFoundDataSetSpecificationException notFoundDataSetSpecificationException)
            {
                throw CreateAndLogValidationException(notFoundDataSetSpecificationException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetSpecificationException =
                    new AlreadyExistsDataSetSpecificationException(
                        message: "DataSetSpecification with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataSetSpecificationException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetSpecificationReferenceException =
                    new InvalidDataSetSpecificationReferenceException(
                        message: "Invalid dataSetSpecification reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataSetSpecificationReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataSetSpecificationException =
                    new LockedDataSetSpecificationException(
                        message: "Locked dataSetSpecification record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedDataSetSpecificationException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataSetSpecificationStorageException =
                    new FailedDataSetSpecificationStorageException(
                        message: "Failed dataSetSpecification storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedDataSetSpecificationStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationServiceException =
                    new FailedDataSetSpecificationServiceException(
                        message: "Failed dataSetSpecification service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetSpecificationServiceException);
            }
        }

        private async ValueTask<IQueryable<DataSetSpecification>> TryCatch(
            ReturningDataSetSpecificationsFunction returningDataSetSpecificationsFunction)
        {
            try
            {
                return await returningDataSetSpecificationsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataSetSpecificationStorageException =
                    new FailedDataSetSpecificationStorageException(
                        message: "Failed dataSetSpecification storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetSpecificationStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationServiceException =
                    new FailedDataSetSpecificationServiceException(
                        message: "Failed dataSetSpecification service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetSpecificationServiceException);
            }
        }

        private DataSetSpecificationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationValidationException);

            return dataSetSpecificationValidationException;
        }

        private DataSetSpecificationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(dataSetSpecificationDependencyException);

            return dataSetSpecificationDependencyException;
        }

        private DataSetSpecificationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationDependencyValidationException);

            return dataSetSpecificationDependencyValidationException;
        }

        private DataSetSpecificationDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var dataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationDependencyException);

            return dataSetSpecificationDependencyException;
        }

        private DataSetSpecificationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataSetSpecificationServiceException =
                new DataSetSpecificationServiceException(
                    message: "DataSetSpecification service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationServiceException);

            return dataSetSpecificationServiceException;
        }
    }
}