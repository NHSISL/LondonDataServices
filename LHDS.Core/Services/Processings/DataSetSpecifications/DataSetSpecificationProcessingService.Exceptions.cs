// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.DataSetSpecifications.Exceptions;
using LHDS.Core.Models.Processings.DataSetSpecifications.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingService : IDataSetSpecificationProcessingService
    {
        private delegate ValueTask<T> ReturningDataSetSpecificationProcessingFunction<T>();
        private delegate DataSetSpecification ReturningSingleDataSetSpecificationProcessingFunction();

        private async ValueTask<T> TryCatch<T>(
            ReturningDataSetSpecificationProcessingFunction<T> returningDataSetSpecificationProcessingFunction)
        {
            try
            {
                return await returningDataSetSpecificationProcessingFunction();
            }
            catch (NullDataSetSpecificationProcessingException nullDataSetSpecificationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDataSetSpecificationException);
            }
            catch (InvalidCountDataSetSpecificationProcessingException invalidCountDataSetSpecificationProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidCountDataSetSpecificationProcessingException);
            }
            catch (InvalidArgumentDataSetSpecificationProcessingException invalidArgumentDataSetSpecificationProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDataSetSpecificationProcessingException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetSpecificationValidationException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetSpecificationDependencyValidationException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationProcessingServiceException =
                    new FailedDataSetSpecificationProcessingServiceException(
                        message: "Failed DataSetSpecification processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDataSetSpecificationProcessingServiceException);
            }
        }

        private async ValueTask<DataSetSpecificationProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationProcessingValidationExceptionn =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingValidationExceptionn);

            return dataSetSpecificationProcessingValidationExceptionn;
        }

        private async ValueTask<DataSetSpecificationProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyValidationException);

            return dataSetSpecificationProcessingDependencyValidationException;
        }

        private async ValueTask<DataSetSpecificationProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyException);

            return dataSetSpecificationProcessingDependencyException;
        }

        private async ValueTask<DataSetSpecificationProcessingServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationProcessingServiceException = new
                DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingServiceException);

            return dataSetSpecificationProcessingServiceException;
        }
    }
}
