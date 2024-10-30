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
        private delegate ValueTask<SpecificationObject> ReturningSpecificationObjectFunction();
        private delegate ValueTask<IQueryable<SpecificationObject>> ReturningSpecificationObjectsFunction();

        private async ValueTask<SpecificationObject> TryCatch(
           ReturningSpecificationObjectFunction returningSpecificationObjectFunction)
        {
            try
            {
                return await returningSpecificationObjectFunction();
            }
            catch (NullSpecificationObjectProcessingException nullSpecificationObjectProcessingException)
            {
                throw CreateAndLogValidationException(nullSpecificationObjectProcessingException);
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectValidationException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                throw CreateAndLogDependencyException(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                throw CreateAndLogDependencyException(specificationObjectServiceException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectProcessingServiceException =
                    new FailedSpecificationObjectProcessingServiceException(
                        message: "Failed SpecificationObject processing service error occurred, " +
                        "please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSpecificationObjectProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<SpecificationObject>> TryCatch(
            ReturningSpecificationObjectsFunction returningSpecificationObjectsFunction)
        {
            try
            {
                return await returningSpecificationObjectsFunction();
            }
            catch (SpecificationObjectValidationException specificationObjectValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectValidationException);
            }
            catch (SpecificationObjectDependencyValidationException specificationObjectDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(specificationObjectDependencyValidationException);
            }
            catch (SpecificationObjectDependencyException specificationObjectDependencyException)
            {
                throw CreateAndLogDependencyException(specificationObjectDependencyException);
            }
            catch (SpecificationObjectServiceException specificationObjectServiceException)
            {
                throw CreateAndLogDependencyException(specificationObjectServiceException);
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
            var specificationObjectProcessingValidationExceptionn =
                new SpecificationObjectProcessingValidationException(
                    message: "SpecificationObject processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectProcessingValidationExceptionn);

            return specificationObjectProcessingValidationExceptionn;
        }

        private SpecificationObjectProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var specificationObjectProcessingDependencyValidationException =
                new SpecificationObjectProcessingDependencyValidationException(
                    message: "SpecificationObject processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(specificationObjectProcessingDependencyValidationException);

            return specificationObjectProcessingDependencyValidationException;
        }

        private SpecificationObjectProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var specificationObjectProcessingDependencyException =
                new SpecificationObjectProcessingDependencyException(
                    message: "SpecificationObject processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(specificationObjectProcessingDependencyException);

            throw specificationObjectProcessingDependencyException;
        }

        private SpecificationObjectProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var specificationObjectProcessingServiceException = new
                SpecificationObjectProcessingServiceException(
                    message: "SpecificationObject processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectProcessingServiceException);

            return specificationObjectProcessingServiceException;
        }
    }
}
