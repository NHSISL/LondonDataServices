// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Models.Processings.SpecificationObjects.Exceptions;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Processings.SpecificationObjects
{
    internal partial class SpecificationObjectProcessingService
    {
        private delegate ValueTask<IQueryable<SpecificationObject>> ReturningSpecificationObjectsFunction();


        private async ValueTask<IQueryable<SpecificationObject>> TryCatch(
            ReturningSpecificationObjectsFunction returningSpecificationObjectsFunction)
        {
            try
            {
                return await returningSpecificationObjectsFunction();
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

            this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingValidationExceptionn);

            return dataSetSpecificationProcessingValidationExceptionn;
        }

        private SpecificationObjectProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyValidationException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyValidationException);

            return dataSetSpecificationProcessingDependencyValidationException;
        }

        private SpecificationObjectProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var dataSetSpecificationProcessingDependencyException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingDependencyException);

            throw dataSetSpecificationProcessingDependencyException;
        }

        private SpecificationObjectProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var dataSetSpecificationProcessingServiceException = new
                SpecificationObjectProcessingServiceException(
                    message: "SpecificationObject processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(dataSetSpecificationProcessingServiceException);

            return dataSetSpecificationProcessingServiceException;
        }
    }
}
