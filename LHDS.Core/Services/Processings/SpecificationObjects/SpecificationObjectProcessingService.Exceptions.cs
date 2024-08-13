// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingService
    {
        private delegate ValueTask<T> ReturningSpecificationObjectProcessingFunction<T>();
        private delegate IQueryable<SpecificationObject> ReturningSpecificationObjectsFunction();
        private delegate SpecificationObject ReturningSingleSpecificationObjectProcessingFunction();

        private async ValueTask<T> TryCatch<T>(
            ReturningSpecificationObjectProcessingFunction<T> returningSpecificationObjectProcessingFunction)
        {
            try
            {
                return await returningSpecificationObjectProcessingFunction();
            }
            catch (InvalidArgumentSpecificationObjectProcessingException invalidArgumentSpecificationObjectProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentSpecificationObjectProcessingException);
            }
            catch (SpecificationObjectValidationException dataSetSpecificationValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationValidationException);
            }
            catch (SpecificationObjectDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException dataSetSpecificationDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationDependencyException);
            }
            catch (SpecificationObjectServiceException dataSetSpecificationServiceException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectProcessingServiceException =
                    new FailedSpecificationObjectProcessingServiceException(
                        message: "Failed SpecificationObject processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSpecificationObjectProcessingServiceException);
            }
        }

        private IQueryable<SpecificationObject> TryCatch(
            ReturningSpecificationObjectsFunction returningSpecificationObjectsFunction)
        {
            try
            {
                return returningSpecificationObjectsFunction();
            }
            catch (SpecificationObjectValidationException dataSetSpecificationValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationValidationException);
            }
            catch (SpecificationObjectDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetSpecificationDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException dataSetSpecificationDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationDependencyException);
            }
            catch (SpecificationObjectServiceException dataSetSpecificationServiceException)
            {
                throw CreateAndLogDependencyException(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectProcessingServiceException =
                    new FailedSpecificationObjectProcessingServiceException(
                        message: "Failed SpecificationObject processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSpecificationObjectProcessingServiceException);
            }
        }

        private SpecificationObjectProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetSpecificationProcessingValidationExceptionn =
                new SpecificationObjectProcessingValidationException(
                    message: "SpecificationObject processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationProcessingValidationExceptionn);

            return dataSetSpecificationProcessingValidationExceptionn;
        }

        private SpecificationObjectProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyValidationException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetSpecificationProcessingDependencyValidationException);

            return dataSetSpecificationProcessingDependencyValidationException;
        }

        private SpecificationObjectProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetSpecificationProcessingDependencyException);

            throw dataSetSpecificationProcessingDependencyException;
        }

        private SpecificationObjectProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var dataSetSpecificationProcessingServiceException = new
                SpecificationObjectProcessingServiceException(
                    message: "SpecificationObject processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetSpecificationProcessingServiceException);

            return dataSetSpecificationProcessingServiceException;
        }
    }
}
