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
                throw await CreateAndLogValidationExceptionAsync(nullDataSetSpecificationException);
            }
            catch (InvalidDataSetSpecificationException invalidDataSetSpecificationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDataSetSpecificationException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetSpecificationStorageException =
                    new FailedDataSetSpecificationStorageException(
                        message: "Failed dataSetSpecification storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataSetSpecificationStorageException);
            }
            catch (NotFoundDataSetSpecificationException notFoundDataSetSpecificationException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDataSetSpecificationException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetSpecificationException =
                    new AlreadyExistsDataSetSpecificationException(
                        message: "DataSetSpecification with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsDataSetSpecificationException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetSpecificationReferenceException =
                    new InvalidDataSetSpecificationReferenceException(
                        message: "Invalid dataSetSpecification reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDataSetSpecificationReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataSetSpecificationException =
                    new LockedDataSetSpecificationException(
                        message: "Locked dataSetSpecification record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedDataSetSpecificationException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataSetSpecificationStorageException =
                    new FailedDataSetSpecificationStorageException(
                        message: "Failed dataSetSpecification storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedDataSetSpecificationStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationServiceException =
                    new FailedDataSetSpecificationServiceException(
                        message: "Failed dataSetSpecification service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetSpecificationServiceException);
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

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataSetSpecificationStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationServiceException =
                    new FailedDataSetSpecificationServiceException(
                        message: "Failed dataSetSpecification service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetSpecificationServiceException);
            }
        }

        private async ValueTask<DataSetSpecificationValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationValidationException =
                new DataSetSpecificationValidationException(
                    message: "DataSetSpecification validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationValidationException);

            return dataSetSpecificationValidationException;
        }

        private async ValueTask<DataSetSpecificationDependencyException> CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(dataSetSpecificationDependencyException);

            return dataSetSpecificationDependencyException;
        }

        private async ValueTask<DataSetSpecificationDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationDependencyValidationException =
                new DataSetSpecificationDependencyValidationException(
                    message: "DataSetSpecification dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationDependencyValidationException);

            return dataSetSpecificationDependencyValidationException;
        }

        private async ValueTask<DataSetSpecificationDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var dataSetSpecificationDependencyException =
                new DataSetSpecificationDependencyException(
                    message: "DataSetSpecification dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationDependencyException);

            return dataSetSpecificationDependencyException;
        }

        private async ValueTask<DataSetSpecificationServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var dataSetSpecificationServiceException =
                new DataSetSpecificationServiceException(
                    message: "DataSetSpecification service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationServiceException);

            return dataSetSpecificationServiceException;
        }
    }
}