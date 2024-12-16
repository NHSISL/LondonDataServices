// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets.Exceptions;
using LHDS.ConfigImportExportTool.Models.Processings.DataSets.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Processings.DataSets
{
    internal partial class DataSetProcessingService
    {
        private delegate ValueTask<IQueryable<DataSet>> ReturningDataSetsFunction();

        private async ValueTask<IQueryable<DataSet>> TryCatch(ReturningDataSetsFunction returningDataSetsFunction)
        {
            try
            {
                return await returningDataSetsFunction();
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetValidationException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetDependencyValidationException);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                throw CreateAndLogDependencyException(dataSetServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetProcessingServiceException =
                    new FailedDataSetProcessingServiceException(
                        message: "Failed DataSet processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetProcessingServiceException);
            }
        }


        private DataSetProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetProcessingValidationExceptionn =
                new DataSetProcessingValidationException(
                    message: "DataSet processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(dataSetProcessingValidationExceptionn);

            return dataSetProcessingValidationExceptionn;
        }

        private DataSetProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(dataSetProcessingDependencyValidationException);

            return dataSetProcessingDependencyValidationException;
        }

        private DataSetProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var dataSetProcessingDependencyException =
                new DataSetProcessingDependencyException(
                    message: "DataSet processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(dataSetProcessingDependencyException);

            throw dataSetProcessingDependencyException;
        }

        private DataSetProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var dataSetProcessingServiceException = new
                DataSetProcessingServiceException(
                    message: "DataSet processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(dataSetProcessingServiceException);

            return dataSetProcessingServiceException;
        }
    }
}
