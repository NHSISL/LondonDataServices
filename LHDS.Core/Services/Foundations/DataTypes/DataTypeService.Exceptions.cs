using System.Threading.Tasks;
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
                    new FailedDataTypeStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataTypeStorageException);
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
    }
}