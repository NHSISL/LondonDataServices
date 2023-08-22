using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService
    {
        private delegate ValueTask<DataType> ReturningDataTypeFunction();

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
                        message: "Failed dataType storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
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
                    message: "DataType dependency error occurred, contact support.",
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
    }
}