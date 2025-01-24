// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        private delegate ValueTask<IQueryable<DataSet>> ReturningDataSetsFunction();

        private async ValueTask<DataSet> TryCatch(ReturningDataSetProcessingFunction returningDataSetProcessingFunction)
        {
            try
            {
                return await returningDataSetProcessingFunction();
            }
            catch (NullDataSetProcessingException nullDataSetException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDataSetException);
            }
            catch (InvalidArgumentDataSetProcessingException invalidArgumentDataSetProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDataSetProcessingException);
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetValidationException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetDependencyValidationException);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetProcessingServiceException =
                    new FailedDataSetProcessingServiceException(
                        message: "Failed DataSet processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<DataSet>> TryCatch(ReturningDataSetsFunction returningDataSetsFunction)
        {
            try
            {
                return await returningDataSetsFunction();
            }
            catch (DataSetValidationException dataSetValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetValidationException);
            }
            catch (DataSetDependencyValidationException dataSetDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetDependencyValidationException);
            }
            catch (DataSetDependencyException dataSetDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetDependencyException);
            }
            catch (DataSetServiceException dataSetServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetProcessingServiceException =
                    new FailedDataSetProcessingServiceException(
                        message: "Failed DataSet processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetProcessingServiceException);
            }
        }

        private async ValueTask<DataSetProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var dataSetProcessingValidationExceptionn =
                new DataSetProcessingValidationException(
                    message: "DataSet processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetProcessingValidationExceptionn);

            return dataSetProcessingValidationExceptionn;
        }

        private async ValueTask<DataSetProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetProcessingDependencyValidationException =
                new DataSetProcessingDependencyValidationException(
                    message: "DataSet processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetProcessingDependencyValidationException);

            return dataSetProcessingDependencyValidationException;
        }

        private async ValueTask<DataSetProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var dataSetProcessingDependencyException =
                new DataSetProcessingDependencyException(
                    message: "DataSet processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetProcessingDependencyException);

            return dataSetProcessingDependencyException;
        }

        private async ValueTask<DataSetProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var dataSetProcessingServiceException = new
                DataSetProcessingServiceException(
                    message: "DataSet processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetProcessingServiceException);

            return dataSetProcessingServiceException;
        }
    }
}
