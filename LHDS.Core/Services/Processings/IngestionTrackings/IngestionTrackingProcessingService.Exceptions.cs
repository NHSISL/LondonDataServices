// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
        private delegate ValueTask<List<string>> ReturningStringListProcessingFunction();
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
            catch (InvalidArgumentIngestionTrackingProcessingException
                invalidArgumentIngestionTrackingProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                Console.Write($"Landings debug: {ingestionTrackingValidationException.Data.ToString()}");
                throw CreateAndLogDependencyValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingProcessingServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(
            ReturningStringListProcessingFunction returningStringListProcessingFunction)
        {
            try
            {
                return await returningStringListProcessingFunction();
            }
            catch (NullIngestionTrackingProcessingException nullIngestionTrackingException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingException);
            }
            catch (InvalidArgumentIngestionTrackingProcessingException
                invalidArgumentIngestionTrackingProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingProcessingServiceException);
            }
        }

        private IQueryable<IngestionTracking> TryCatch(ReturningIngestionTrackingsFunction
            returningIngestionTrackingsFunction)
        {
            try
            {
                return returningIngestionTrackingsFunction();
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingProcessingServiceException);
            }
        }


        private IngestionTrackingProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingProcessingValidationExceptionn =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingProcessingValidationExceptionn);

            return ingestionTrackingProcessingValidationExceptionn;
        }

        private IngestionTrackingProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var ingestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(ingestionTrackingProcessingDependencyValidationException);

            return ingestionTrackingProcessingDependencyValidationException;
        }

        private IngestionTrackingProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var ingestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(ingestionTrackingProcessingDependencyException);

            throw ingestionTrackingProcessingDependencyException;
        }

        private IngestionTrackingProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ingestionTrackingProcessingServiceException = new
                IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingProcessingServiceException);

            return ingestionTrackingProcessingServiceException;
        }
    }
}
