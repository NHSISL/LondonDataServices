// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private delegate ValueTask<DataType> ReturningDataTypeFunction();
        private delegate ValueTask<IQueryable<DataType>> ReturningDataTypesFunction();

        private async ValueTask<DataType> TryCatch(ReturningDataTypeFunction returningDataTypeFunction)
        {
            try
            {
                return await returningDataTypeFunction();
            }
            catch (NullDataTypeException nullDataTypeException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDataTypeException);
            }
            catch (InvalidDataTypeException invalidDataTypeException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDataTypeException);
            }
            catch (SqlException sqlException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataTypeStorageException);
            }
            catch (NotFoundDataTypeException notFoundDataTypeException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDataTypeException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataTypeException =
                    new AlreadyExistsDataTypeException(
                        message: "DataType with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsDataTypeException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataTypeReferenceException =
                    new InvalidDataTypeReferenceException(
                        message: "Invalid dataType reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDataTypeReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataTypeException =
                    new LockedDataTypeException(
                        message: "Locked dataType record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedDataTypeException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(
                        message: "Failed dataType service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataTypeServiceException);
            }
        }

        private async ValueTask<IQueryable<DataType>> TryCatch(ReturningDataTypesFunction returningDataTypesFunction)
        {
            try
            {
                return await returningDataTypesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(
                        message: "Failed dataType service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataTypeServiceException);
            }
        }

        private async ValueTask<DataTypeValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var dataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataTypeValidationException);

            return dataTypeValidationException;
        }

        private async ValueTask<DataTypeDependencyException> CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var dataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private async ValueTask<DataTypeDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: "DataType dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataTypeDependencyValidationException);

            return dataTypeDependencyValidationException;
        }

        private async ValueTask<DataTypeDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var dataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private async ValueTask<DataTypeServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var dataTypeServiceException =
                new DataTypeServiceException(
                    message: "DataType service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataTypeServiceException);

            return dataTypeServiceException;
        }
    }
}