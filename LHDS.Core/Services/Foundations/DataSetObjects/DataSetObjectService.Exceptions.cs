using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.DataSetObjects;
using LHDS.Core.Models.Foundations.DataSetObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService
    {
        private delegate ValueTask<DataSetObject> ReturningDataSetObjectFunction();

        private async ValueTask<DataSetObject> TryCatch(ReturningDataSetObjectFunction returningDataSetObjectFunction)
        {
            try
            {
                return await returningDataSetObjectFunction();
            }
            catch (NullDataSetObjectException nullDataSetObjectException)
            {
                throw CreateAndLogValidationException(nullDataSetObjectException);
            }
            catch (InvalidDataSetObjectException invalidDataSetObjectException)
            {
                throw CreateAndLogValidationException(invalidDataSetObjectException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetObjectStorageException =
                    new FailedDataSetObjectStorageException(
                        message: "Failed dataSetObject storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetObjectStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetObjectException =
                    new AlreadyExistsDataSetObjectException(
                        message: "DataSetObject with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataSetObjectException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetObjectReferenceException =
                    new InvalidDataSetObjectReferenceException(
                        message: "Invalid dataSetObject reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataSetObjectReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDataSetObjectStorageException =
                    new FailedDataSetObjectStorageException(
                        message: "Failed dataSetObject storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedDataSetObjectStorageException);
            }
            catch (Exception exception)
            {
                var failedDataSetObjectServiceException =
                    new FailedDataSetObjectServiceException(
                        message: "Failed dataSetObject service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetObjectServiceException);
            }
        }

        private DataSetObjectValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetObjectValidationException =
                new DataSetObjectValidationException(
                    message: "DataSetObject validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetObjectValidationException);

            return dataSetObjectValidationException;
        }

        private DataSetObjectDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataSetObjectDependencyException = 
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(dataSetObjectDependencyException);

            return dataSetObjectDependencyException;
        }

        private DataSetObjectDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataSetObjectDependencyValidationException =
                new DataSetObjectDependencyValidationException(
                    message: "DataSetObject dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetObjectDependencyValidationException);

            return dataSetObjectDependencyValidationException;
        }

        private DataSetObjectDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var dataSetObjectDependencyException = 
                new DataSetObjectDependencyException(
                    message: "DataSetObject dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(dataSetObjectDependencyException);

            return dataSetObjectDependencyException;
        }

        private DataSetObjectServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var dataSetObjectServiceException = 
                new DataSetObjectServiceException(
                    message: "DataSetObject service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetObjectServiceException);

            return dataSetObjectServiceException;
        }
    }
}