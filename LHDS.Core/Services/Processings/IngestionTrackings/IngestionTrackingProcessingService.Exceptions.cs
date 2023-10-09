// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingService : IIngestionTrackingProcessingService
    {
        private delegate ValueTask<IngestionTracking> ReturningIngestionTrackingProcessingFunction();
        private delegate IQueryable<IngestionTracking> ReturningIngestionTrackingsFunction();

        private async ValueTask<IngestionTracking> TryCatch(
            ReturningIngestionTrackingProcessingFunction returningIngestionTrackingProcessingFunction)
        {
            try
            {
                return await returningIngestionTrackingProcessingFunction();
            }
            catch (NullIngestionTrackingProcessingException nullIngestionTrackingException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingException);
            }
            catch (InvalidArgumentIngestionTrackingProcessingException invalidArgumentIngestionTrackingProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException objectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnValidationException);
            }
            catch (IngestionTrackingDependencyValidationException objectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException objectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(objectColumnDependencyException);
            }
            catch (IngestionTrackingServiceException objectColumnServiceException)
            {
                throw CreateAndLogDependencyException(objectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingProcessingServiceException);
            }
        }

        private IQueryable<IngestionTracking> TryCatch(ReturningIngestionTrackingsFunction returningIngestionTrackingsFunction)
        {
            try
            {
                return returningIngestionTrackingsFunction();
            }
            catch (IngestionTrackingValidationException objectColumnValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnValidationException);
            }
            catch (IngestionTrackingDependencyValidationException objectColumnDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(objectColumnDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException objectColumnDependencyException)
            {
                throw CreateAndLogDependencyException(objectColumnDependencyException);
            }
            catch (IngestionTrackingServiceException objectColumnServiceException)
            {
                throw CreateAndLogDependencyException(objectColumnServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingProcessingServiceException);
            }
        }


        private IngestionTrackingProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var objectColumnProcessingValidationExceptionn =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnProcessingValidationExceptionn);

            return objectColumnProcessingValidationExceptionn;
        }

        private IngestionTrackingProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var objectColumnProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(objectColumnProcessingDependencyValidationException);

            return objectColumnProcessingDependencyValidationException;
        }

        private IngestionTrackingProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var objectColumnProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(objectColumnProcessingDependencyException);

            throw objectColumnProcessingDependencyException;
        }

        private IngestionTrackingProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var objectColumnProcessingServiceException = new
                IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnProcessingServiceException);

            return objectColumnProcessingServiceException;
        }
    }
}
