// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
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
            catch (NullDataSetProcessingException nullDocumentException)
            {
                throw CreateAndLogValidationException(nullDocumentException);
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
    }
}
