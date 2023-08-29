using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService
    {
        private delegate ValueTask<DataSetSpecification> ReturningDataSetSpecificationFunction();

        private async ValueTask<DataSetSpecification> TryCatch(ReturningDataSetSpecificationFunction returningDataSetSpecificationFunction)
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
                        message: "Failed dataSetSpecification storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetSpecificationStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetSpecificationException =
                    new AlreadyExistsDataSetSpecificationException(
                        message: "DataSetSpecification with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataSetSpecificationException);
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
                    message: "DataSetSpecification dependency error occurred, contact support.",
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
    }
}