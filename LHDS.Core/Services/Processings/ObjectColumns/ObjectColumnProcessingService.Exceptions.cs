// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using LHDS.Core.Models.Processings.ObjectColumns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingService : IObjectColumnProcessingService
    {
        private delegate ValueTask<ObjectColumn> ReturningObjectColumnProcessingFunction();
        private delegate ValueTask<IQueryable<ObjectColumn>> ReturningObjectColumnsFunction();

        private async ValueTask<ObjectColumn> TryCatch(
            ReturningObjectColumnProcessingFunction returningObjectColumnProcessingFunction)
        {
            try
            {
                return await returningObjectColumnProcessingFunction();
            }
            catch (NullObjectColumnProcessingException nullObjectColumnException)
            {
                throw CreateAndLogValidationException(nullObjectColumnException);
            }
            catch (InvalidArgumentObjectColumnProcessingException invalidArgumentObjectColumnProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentObjectColumnProcessingException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                throw CreateAndLogDependencyException(objectColumnServiceException);
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

        private ValueTask<IQueryable<ObjectColumn>> TryCatch(
            ReturningObjectColumnsFunction returningObjectColumnsFunction)
        {
            try
            {
                return returningObjectColumnsFunction();
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                throw CreateAndLogDependencyException(objectColumnServiceException);
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
            var objectColumnProcessingValidationExceptionn =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnProcessingValidationExceptionn);

            return objectColumnProcessingValidationExceptionn;
        }

        private ObjectColumnProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var objectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(objectColumnProcessingDependencyValidationException);

            return objectColumnProcessingDependencyValidationException;
        }

        private ObjectColumnProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var objectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(objectColumnProcessingDependencyException);

            throw objectColumnProcessingDependencyException;
        }

        private ObjectColumnProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var objectColumnProcessingServiceException = new
                ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnProcessingServiceException);

            return objectColumnProcessingServiceException;
        }
    }
}
