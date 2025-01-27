// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using LHDS.Core.Models.Processings.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SpecificationObjects
{
    public partial class SpecificationObjectProcessingService
    {
        private delegate ValueTask<T> ReturningSpecificationObjectProcessingFunction<T>();

        private async ValueTask<T> TryCatch<T>(
            ReturningSpecificationObjectProcessingFunction<T> returningSpecificationObjectProcessingFunction)
        {
            try
            {
                return await returningSpecificationObjectProcessingFunction();
            }
            catch (InvalidArgumentSpecificationObjectProcessingException invalidArgumentSpecificationObjectProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentSpecificationObjectProcessingException);
            }
            catch (SpecificationObjectValidationException dataSetSpecificationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(dataSetSpecificationValidationException);
            }
            catch (SpecificationObjectDependencyValidationException dataSetSpecificationDependencyValidationException)
            {
                throw await
                    CreateAndLogDependencyValidationExceptionAsync(dataSetSpecificationDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException dataSetSpecificationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetSpecificationDependencyException);
            }
            catch (SpecificationObjectServiceException dataSetSpecificationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(dataSetSpecificationServiceException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectProcessingServiceException =
                    new FailedSpecificationObjectProcessingServiceException(
                        message:
                            "Failed SpecificationObject processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSpecificationObjectProcessingServiceException);
            }
        }

        private async ValueTask<SpecificationObjectProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var dataSetSpecificationProcessingValidationExceptionn =
                new SpecificationObjectProcessingValidationException(
                    message: "SpecificationObject processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingValidationExceptionn);

            return dataSetSpecificationProcessingValidationExceptionn;
        }

        private async ValueTask<SpecificationObjectProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyValidationException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyValidationException);

            return dataSetSpecificationProcessingDependencyValidationException;
        }

        private async ValueTask<SpecificationObjectProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyException);

            return dataSetSpecificationProcessingDependencyException;
        }

        private async ValueTask<SpecificationObjectProcessingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var dataSetSpecificationProcessingServiceException = new
                SpecificationObjectProcessingServiceException(
                    message: "SpecificationObject processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingServiceException);

            return dataSetSpecificationProcessingServiceException;
        }
    }
}
