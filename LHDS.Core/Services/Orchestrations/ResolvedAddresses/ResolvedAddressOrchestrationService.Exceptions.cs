// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<List<Guid>> ReturningGuidListFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentResolvedAddressOrchestrationException
                invalidArgumentResolvedAddressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressOrchestrationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentProcessingDependencyValidationException);
            }
            catch (ResolvedAddressProcessingValidationException resolvedAddressProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressProcessingValidationException);
            }
            catch (ResolvedAddressProcessingDependencyValidationException
                resolvedAddressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw CreateAndLogDependencyException(documentProcessingServiceException);
            }
            catch (ResolvedAddressProcessingDependencyException resolvedAddressProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingDependencyException);
            }
            catch (ResolvedAddressProcessingServiceException resolvedAddressProcessingServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedResolvedAddressOrchestrationServiceException);
            }
        }

        private async ValueTask<List<Guid>> TryCatch(ReturningGuidListFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (InvalidArgumentResolvedAddressOrchestrationException
                invalidArgumentResolvedAddressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressOrchestrationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentProcessingDependencyValidationException);
            }
            catch (ResolvedAddressProcessingValidationException resolvedAddressProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressProcessingValidationException);
            }
            catch (ResolvedAddressProcessingDependencyValidationException
                resolvedAddressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw CreateAndLogDependencyException(documentProcessingServiceException);
            }
            catch (ResolvedAddressProcessingDependencyException resolvedAddressProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingDependencyException);
            }
            catch (ResolvedAddressProcessingServiceException resolvedAddressProcessingServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedFailedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedFailedResolvedAddressOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedResolvedAddressOrchestrationServiceException);
            }
        }

        private ResolvedAddressOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var resolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressOrchestrationValidationException);

            return resolvedAddressOrchestrationValidationException;
        }

        private ResolvedAddressOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var resolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation error occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(resolvedAddressOrchestrationDependencyValidationException);

            return resolvedAddressOrchestrationDependencyValidationException;
        }

        private ResolvedAddressOrchestrationDependencyException
           CreateAndLogDependencyException(Xeption exception)
        {
            var resolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(resolvedAddressOrchestrationDependencyException);

            return resolvedAddressOrchestrationDependencyException;
        }

        private ResolvedAddressOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var resolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(resolvedAddressOrchestrationServiceException);

            return resolvedAddressOrchestrationServiceException;
        }
    }
}
