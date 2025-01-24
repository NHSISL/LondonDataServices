// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.TerminologyDetails
{
    internal partial class TerminologyDetailOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (DocumentProcessingValidationException documentProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentProcessingDependencyValidationException);
            }
            catch (TerminologyArtifactProcessingValidationException terminologyArtifactProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(terminologyArtifactProcessingValidationException);
            }
            catch (TerminologyArtifactProcessingDependencyValidationException
                terminologyArtifactProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyArtifactProcessingDependencyValidationException);
            }
            catch (OntologyProcessingValidationException ontologyProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyProcessingValidationException);
            }
            catch (OntologyProcessingDependencyValidationException ontologyProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyProcessingDependencyValidationException);
            }
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentProcessingServiceException);
            }
            catch (TerminologyArtifactProcessingDependencyException terminologyArtifactProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactProcessingDependencyException);
            }
            catch (TerminologyArtifactProcessingServiceException terminologyArtifactProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyArtifactProcessingServiceException);
            }
            catch (OntologyProcessingDependencyException ontologyProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyProcessingDependencyException);
            }
            catch (OntologyProcessingServiceException ontologyProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyProcessingServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedTerminologyDetailOrchestrationServiceException =
                    new FailedTerminologyDetailOrchestrationServiceException(
                        message: "Failed terminology detail aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyDetailOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var FailedTerminologyDetailOrchestrationServiceException =
                    new FailedTerminologyDetailOrchestrationServiceException(
                        message: "Failed terminology detail orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(FailedTerminologyDetailOrchestrationServiceException);
            }
        }

        private async ValueTask<TerminologyDetailOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyDetailOrchestrationDependencyValidationException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message: "Terminology detail orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyDetailOrchestrationDependencyValidationException);

            return terminologyDetailOrchestrationDependencyValidationException;
        }

        private async ValueTask<TerminologyDetailOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var terminologyDetailOrchestrationDependencyException =
                new TerminologyDetailOrchestrationDependencyException(
                    message: "Terminology detail orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyDetailOrchestrationDependencyException);

            throw terminologyDetailOrchestrationDependencyException;
        }

        private async ValueTask<TerminologyDetailOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(terminologyDetailOrchestrationServiceException);

            throw terminologyDetailOrchestrationServiceException;
        }
    }
}