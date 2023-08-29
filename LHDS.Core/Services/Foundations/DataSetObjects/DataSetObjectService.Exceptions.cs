using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}