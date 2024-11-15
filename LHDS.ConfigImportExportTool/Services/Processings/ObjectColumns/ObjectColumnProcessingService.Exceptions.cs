// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using LHDS.ConfigImportExportTool.Models.Processings.ObjectColumns.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Processings.ObjectColumns
{
    internal partial class ObjectColumnProcessingService
    {
        private delegate ValueTask<ObjectColumn> ReturningObjectColumnFunction();
        private delegate ValueTask<IQueryable<ObjectColumn>> ReturningObjectColumnsFunction();

        private async ValueTask<ObjectColumn> TryCatch(
           ReturningObjectColumnFunction returningObjectColumnFunction)
        {
            try
            {
                return await returningObjectColumnFunction();
            }
            catch (NullObjectColumnProcessingException nullObjectColumnProcessingException)
            {
                throw CreateAndLogValidationException(nullObjectColumnProcessingException);
            }
            catch (ObjectColumnValidationException ObjectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(ObjectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException ObjectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ObjectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException ObjectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(ObjectColumnDependencyException);
            }
            catch (ObjectColumnServiceException ObjectColumnServiceException)
            {
                throw CreateAndLogDependencyException(ObjectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnProcessingServiceException =
                    new FailedObjectColumnProcessingServiceException(
                        message: "Failed ObjectColumn processing service error occurred, " +
                        "please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedObjectColumnProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<ObjectColumn>> TryCatch(
            ReturningObjectColumnsFunction returningObjectColumnsFunction)
        {
            try
            {
                return await returningObjectColumnsFunction();
            }
            catch (ObjectColumnValidationException ObjectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(ObjectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException ObjectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ObjectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException ObjectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(ObjectColumnDependencyException);
            }
            catch (ObjectColumnServiceException ObjectColumnServiceException)
            {
                throw CreateAndLogDependencyException(ObjectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnProcessingServiceException =
                    new FailedObjectColumnProcessingServiceException(
                        message: "Failed ObjectColumn processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedObjectColumnProcessingServiceException);
            }
        }

        private ObjectColumnProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ObjectColumnProcessingValidationExceptionn =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(ObjectColumnProcessingValidationExceptionn);

            return ObjectColumnProcessingValidationExceptionn;
        }

        private ObjectColumnProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var ObjectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(ObjectColumnProcessingDependencyValidationException);

            return ObjectColumnProcessingDependencyValidationException;
        }

        private ObjectColumnProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var ObjectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogErrorAsync(ObjectColumnProcessingDependencyException);

            throw ObjectColumnProcessingDependencyException;
        }

        private ObjectColumnProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ObjectColumnProcessingServiceException = new
                ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(ObjectColumnProcessingServiceException);

            return ObjectColumnProcessingServiceException;
        }
    }
}
