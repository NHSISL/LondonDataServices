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
                throw await CreateAndLogValidationExceptionAsync(nullObjectColumnException);
            }
            catch (InvalidArgumentObjectColumnProcessingException invalidArgumentObjectColumnProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentObjectColumnProcessingException);
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(objectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(objectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                throw await CreateAndLogServiceExceptionAsync(objectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnProcessingServiceException =
                    new FailedObjectColumnProcessingServiceException(
                        message: "Failed ObjectColumn processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedObjectColumnProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<ObjectColumn>> TryCatch(
            ReturningObjectColumnsFunction returningObjectColumnsFunction)
        {
            try
            {
                return await returningObjectColumnsFunction();
            }
            catch (ObjectColumnValidationException objectColumnValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(objectColumnValidationException);
            }
            catch (ObjectColumnDependencyValidationException objectColumnDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(objectColumnDependencyValidationException);
            }
            catch (ObjectColumnDependencyException objectColumnDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(objectColumnDependencyException);
            }
            catch (ObjectColumnServiceException objectColumnServiceException)
            {
                throw await CreateAndLogServiceExceptionAsync(objectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnProcessingServiceException =
                    new FailedObjectColumnProcessingServiceException(
                        message: "Failed ObjectColumn processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedObjectColumnProcessingServiceException);
            }
        }

        private async ValueTask<ObjectColumnProcessingValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var objectColumnProcessingValidationExceptionn =
                new ObjectColumnProcessingValidationException(
                    message: "ObjectColumn processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnProcessingValidationExceptionn);

            return objectColumnProcessingValidationExceptionn;
        }

        private async ValueTask<ObjectColumnProcessingDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var objectColumnProcessingDependencyValidationException =
                new ObjectColumnProcessingDependencyValidationException(
                    message: "ObjectColumn processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(objectColumnProcessingDependencyValidationException);

            return objectColumnProcessingDependencyValidationException;
        }

        private async ValueTask<ObjectColumnProcessingDependencyException> 
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var objectColumnProcessingDependencyException =
                new ObjectColumnProcessingDependencyException(
                    message: "ObjectColumn processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(objectColumnProcessingDependencyException);

            throw objectColumnProcessingDependencyException;
        }

        private async ValueTask<ObjectColumnProcessingServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var objectColumnProcessingServiceException = new
                ObjectColumnProcessingServiceException(
                    message: "ObjectColumn processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnProcessingServiceException);

            return objectColumnProcessingServiceException;
        }
    }
}
