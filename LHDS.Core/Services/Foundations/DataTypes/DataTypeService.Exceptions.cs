using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
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
                    new FailedDataTypeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
            }
            catch (NotFoundDataTypeException notFoundDataTypeException)
            {
                throw CreateAndLogValidationException(notFoundDataTypeException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataTypeException =
                    new AlreadyExistsDataTypeException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataTypeException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataTypeReferenceException =
                    new InvalidDataTypeReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataTypeReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataTypeStorageException =
                    new FailedDataTypeStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(exception);

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
                    new FailedDataTypeStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
            }
            catch (Exception exception)
            {
                var failedDataTypeServiceException =
                    new FailedDataTypeServiceException(exception);

                throw CreateAndLogServiceException(failedDataTypeServiceException);
            }
        }

        private DataTypeValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataTypeValidationException =
                new DataTypeValidationException(exception);

            this.loggingBroker.LogError(dataTypeValidationException);

            return dataTypeValidationException;
        }

        private DataTypeDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataTypeDependencyException = new DataTypeDependencyException(exception);
            this.loggingBroker.LogCritical(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private DataTypeDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataTypeDependencyValidationException =
                new DataTypeDependencyValidationException(exception);

            this.loggingBroker.LogError(dataTypeDependencyValidationException);

            return dataTypeDependencyValidationException;
        }

        private DataTypeDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var dataTypeDependencyException = new DataTypeDependencyException(exception);
            this.loggingBroker.LogError(dataTypeDependencyException);

            return dataTypeDependencyException;
        }

        private DataTypeServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataTypeServiceException = new DataTypeServiceException(exception);
            this.loggingBroker.LogError(dataTypeServiceException);

            return dataTypeServiceException;
        }
    }
}