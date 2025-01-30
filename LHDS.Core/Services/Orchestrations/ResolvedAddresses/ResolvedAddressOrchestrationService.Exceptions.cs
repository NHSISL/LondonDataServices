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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentResolvedAddressOrchestrationException);
            }
            catch (NullResolvedAddressOrchestrationException nullResolvedAddressOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullResolvedAddressOrchestrationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    documentProcessingDependencyValidationException);
            }
            catch (ResolvedAddressProcessingValidationException resolvedAddressProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressProcessingValidationException);
            }
            catch (ResolvedAddressProcessingDependencyValidationException
                resolvedAddressProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvHelperClientValidationException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingServiceException);
            }
            catch (ResolvedAddressProcessingDependencyException resolvedAddressProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressProcessingDependencyException);
            }
            catch (ResolvedAddressProcessingServiceException resolvedAddressProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressProcessingServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address aggregate orchestration service errors occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressOrchestrationServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentResolvedAddressOrchestrationException);
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    documentProcessingDependencyValidationException);
            }
            catch (ResolvedAddressProcessingValidationException resolvedAddressProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressProcessingValidationException);
            }
            catch (ResolvedAddressProcessingDependencyValidationException
                resolvedAddressProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressProcessingDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvHelperClientValidationException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingServiceException);
            }
            catch (ResolvedAddressProcessingDependencyException resolvedAddressProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressProcessingDependencyException);
            }
            catch (ResolvedAddressProcessingServiceException resolvedAddressProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressProcessingServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedFailedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address aggregate orchestration service errors occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedFailedResolvedAddressOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressOrchestrationServiceException =
                    new FailedResolvedAddressOrchestrationServiceException(
                        message: "Failed resolved address orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressOrchestrationServiceException);
            }
        }

        private async ValueTask<ResolvedAddressOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressOrchestrationValidationException);

            return resolvedAddressOrchestrationValidationException;
        }

        private async ValueTask<ResolvedAddressOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressOrchestrationDependencyValidationException =
                new ResolvedAddressOrchestrationDependencyValidationException(
                    message: "Resolved address orchestration dependency validation errors occurred, please try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(resolvedAddressOrchestrationDependencyValidationException);

            return resolvedAddressOrchestrationDependencyValidationException;
        }

        private async ValueTask<ResolvedAddressOrchestrationDependencyException>
           CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var resolvedAddressOrchestrationDependencyException =
                new ResolvedAddressOrchestrationDependencyException(
                    message: "Resolved address orchestration dependency errors occurred, please contact support.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(resolvedAddressOrchestrationDependencyException);

            return resolvedAddressOrchestrationDependencyException;
        }

        private async ValueTask<ResolvedAddressOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var resolvedAddressOrchestrationServiceException =
                new ResolvedAddressOrchestrationServiceException(
                    message: "Resolved address orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressOrchestrationServiceException);

            return resolvedAddressOrchestrationServiceException;
        }
    }
}
