// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        private delegate IQueryable<DataSetSpecification> ReturningDataSetSpecificationsFunction();
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
                throw CreateAndLogValidationException(nullDataSetSpecificationException);
            }
            catch (InvalidCountDataSetSpecificationProcessingException invalidCountDataSetSpecificationProcessingException)
            {
                throw CreateAndLogValidationException(invalidCountDataSetSpecificationProcessingException);
            }
            catch (InvalidArgumentDataSetSpecificationProcessingException invalidArgumentDataSetSpecificationProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentDataSetSpecificationProcessingException);
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationValidationException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationDependencyValidationException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationProcessingServiceException =
                    new FailedDataSetSpecificationProcessingServiceException(
                        message: "Failed DataSetSpecification processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetSpecificationProcessingServiceException);
            }
        }

        private IQueryable<DataSetSpecification> TryCatch(
            ReturningDataSetSpecificationsFunction returningDataSetSpecificationsFunction)
        {
            try
            {
                return returningDataSetSpecificationsFunction();
            }
            catch (DataSetSpecificationValidationException dataSetSpecificationValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationValidationException);
            }
            catch (DataSetSpecificationDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationDependencyValidationException);
            }
            catch (DataSetSpecificationDependencyException dataSetSpecificationDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationDependencyException);
            }
            catch (DataSetSpecificationServiceException dataSetSpecificationServiceException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedDataSetSpecificationProcessingServiceException =
                    new FailedDataSetSpecificationProcessingServiceException(
                        message: "Failed DataSetSpecification processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDataSetSpecificationProcessingServiceException);
            }
        }

        private DataSetSpecificationProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetSpecificationProcessingValidationExceptionn =
                new DataSetSpecificationProcessingValidationException(
                    message: "DataSetSpecification processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationProcessingValidationExceptionn);

            return dataSetSpecificationProcessingValidationExceptionn;
        }

        private DataSetSpecificationProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyValidationException =
                new DataSetSpecificationProcessingDependencyValidationException(
                    message: "DataSetSpecification processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetSpecificationProcessingDependencyValidationException);

            return dataSetSpecificationProcessingDependencyValidationException;
        }

        private DataSetSpecificationProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyException =
                new DataSetSpecificationProcessingDependencyException(
                    message: "DataSetSpecification processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetSpecificationProcessingDependencyException);

            throw dataSetSpecificationProcessingDependencyException;
        }

        private DataSetSpecificationProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var dataSetSpecificationProcessingServiceException = new
                DataSetSpecificationProcessingServiceException(
                    message: "DataSetSpecification processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationProcessingServiceException);

            return dataSetSpecificationProcessingServiceException;
        }
    }
}
