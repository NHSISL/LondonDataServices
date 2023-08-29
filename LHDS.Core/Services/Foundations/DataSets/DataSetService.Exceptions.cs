using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService
    {
        private delegate ValueTask<DataSet> ReturningDataSetFunction();

        private async ValueTask<DataSet> TryCatch(ReturningDataSetFunction returningDataSetFunction)
        {
            try
            {
                return await returningDataSetFunction();
            }
            catch (NullDataSetException nullDataSetException)
            {
                throw CreateAndLogValidationException(nullDataSetException);
            }
            catch (InvalidDataSetException invalidDataSetException)
            {
                throw CreateAndLogValidationException(invalidDataSetException);
            }
            catch (SqlException sqlException)
            {
                var failedDataSetStorageException =
                    new FailedDataSetStorageException(
                        message: "Failed dataSet storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedDataSetStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDataSetException =
                    new AlreadyExistsDataSetException(
                        message: "DataSet with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsDataSetException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDataSetReferenceException =
                    new InvalidDataSetReferenceException(
                        message: "Invalid dataSet reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidDataSetReferenceException);
            }
        }

        private DataSetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetValidationException =
                new DataSetValidationException(
                    message: "DataSet validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetValidationException);

            return dataSetValidationException;
        }

        private DataSetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var dataSetDependencyException = 
                new DataSetDependencyException(
                    message: "DataSet dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(dataSetDependencyException);

            return dataSetDependencyException;
        }

        private DataSetDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var dataSetDependencyValidationException =
                new DataSetDependencyValidationException(
                    message: "DataSet dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetDependencyValidationException);

            return dataSetDependencyValidationException;
        }
    }
}