// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using LHDS.Core.Models.Processings.DataSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.DataSets
{
    public partial class DataSetProcessingService : IDataSetProcessingService
    {
        private delegate ValueTask<DataSet> ReturningDataSetProcessingFunction();

        private async ValueTask<DataSet> TryCatch(ReturningDataSetProcessingFunction returningDataSetProcessingFunction)
        {
            try
            {
                return await returningDataSetProcessingFunction();
            }
            catch (NullDataSetProcessingException nullDataSetException)
            {
                throw CreateAndLogValidationException(nullDataSetException);
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetValidationException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetDependencyValidationException);
            }
        }

        private DataSetProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetProcessingValidationExceptionn =
                new DataSetProcessingValidationException(
                    message: "DataSet processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetProcessingValidationExceptionn);

            return dataSetProcessingValidationExceptionn;
        }

        private DataSetProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetProcessingDependencyValidationException);

            return dataSetProcessingDependencyValidationException;
        }
    }
}
