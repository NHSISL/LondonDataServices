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
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<List<string>> ReturningStringListProcessingFunction();
        private delegate ValueTask<IQueryable<IngestionTracking>> ReturningIngestionTrackingsFunction();

        private async ValueTask<IngestionTracking> TryCatch(
            ReturningIngestionTrackingProcessingFunction returningIngestionTrackingProcessingFunction)
        {
            try
            {
                return await returningIngestionTrackingProcessingFunction();
            }
            catch (NullIngestionTrackingProcessingException nullIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingException);
            }
            catch (InvalidArgumentIngestionTrackingProcessingException
                invalidArgumentIngestionTrackingProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingProcessingServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullIngestionTrackingProcessingException nullIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingException);
            }
            catch (InvalidArgumentIngestionTrackingProcessingException
                invalidArgumentIngestionTrackingProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingException);
            }
            catch (InvalidArgumentIngestionTrackingProcessingException
                invalidArgumentIngestionTrackingProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentIngestionTrackingProcessingException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<IngestionTracking>> TryCatch(ReturningIngestionTrackingsFunction
            returningIngestionTrackingsFunction)
        {
            try
            {
                return await returningIngestionTrackingsFunction();
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingProcessingServiceException =
                    new FailedIngestionTrackingProcessingServiceException(
                        message: "Failed IngestionTracking processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingProcessingServiceException);
            }
        }


        private async ValueTask<IngestionTrackingProcessingValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingProcessingValidationExceptionn =
                new IngestionTrackingProcessingValidationException(
                    message: "IngestionTracking processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingProcessingValidationExceptionn);

            return ingestionTrackingProcessingValidationExceptionn;
        }

        private async ValueTask<IngestionTrackingProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingProcessingDependencyValidationException);

            return ingestionTrackingProcessingDependencyValidationException;
        }

        private async ValueTask<IngestionTrackingProcessingDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var ingestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingProcessingDependencyException);

            return ingestionTrackingProcessingDependencyException;
        }

        private async ValueTask<IngestionTrackingProcessingServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingProcessingServiceException = new
                IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingProcessingServiceException);

            return ingestionTrackingProcessingServiceException;
        }
    }
}
