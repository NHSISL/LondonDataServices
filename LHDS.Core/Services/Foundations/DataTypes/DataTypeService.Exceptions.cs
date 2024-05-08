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
        private delegate IQueryable<DataType> ReturningDataTypesFunction();

        private async ValueTask<DataType> TryCatch(ReturningDataTypeFunction returningDataTypeFunction)
        {
            try
            {
                return await returningDataTypeFunction();
            }
            catch (NullDataTypeException nullDataTypeException)
            {
                throw CreateAndLogValidationException(nullDataTypeException);
            }
            catch (InvalidDataTypeException invalidDataTypeException)
            {
                throw CreateAndLogValidationException(invalidDataTypeException);
            }
            catch (SqlException sqlException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
            }
            catch (NotFoundDataTypeException notFoundDataTypeException)
            {
                throw CreateAndLogValidationException(notFoundDataTypeException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataTypeException =
                    new AlreadyExistsDataTypeException(
                        message: "DataType with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataTypeException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataTypeReferenceException =
                    new InvalidDataTypeReferenceException(
                        message: "Invalid dataType reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataTypeReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedDataTypeException =
                    new LockedDataTypeException(
                        message: "Locked dataType record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedDataTypeException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(
                        message: "Failed dataType service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataTypeServiceException);
            }
        }

        private IQueryable<DataType> TryCatch(ReturningDataTypesFunction returningDataTypesFunction)
        {
            try
            {
                return returningDataTypesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(
                        message: "Failed dataType storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(
                        message: "Failed dataType service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataTypeServiceException);
            }
        }

        private DataTypeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataTypeValidationException =
                new DataTypeValidationException(
                    message: "DataType validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataTypeValidationException);

            return dataTypeValidationException;
        }

        private DataTypeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private DataTypeDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(
                    message: "DataType dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataTypeDependencyValidationException);

            return dataTypeDependencyValidationException;
        }

        private DataTypeDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var dataTypeDependencyException =
                new DataTypeDependencyException(
                    message: "DataType dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private DataTypeServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataTypeServiceException =
                new DataTypeServiceException(
                    message: "DataType service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataTypeServiceException);

            return dataTypeServiceException;
        }
    }
}