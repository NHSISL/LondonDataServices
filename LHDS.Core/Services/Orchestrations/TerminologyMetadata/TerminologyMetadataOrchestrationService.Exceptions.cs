// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using LHDS.Core.Models.Processings.TerminologyArtifacts.Exceptions;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentTerminologyMetaDataOrchestrationException
                invalidArgumentTerminologyMetaDataOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(
                    invalidArgumentTerminologyMetaDataOrchestrationException);
            }
            catch (TerminologyPollProcessingValidationException terminologyPollProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyPollProcessingValidationException);
            }
            catch (TerminologyPollProcessingDependencyValidationException
                terminologyPollProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyPollProcessingDependencyValidationException);
            }
            catch (TerminologyArtifactProcessingValidationException terminologyArtifactProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    terminologyArtifactProcessingValidationException);
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
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ontologyProcessingDependencyValidationException);
            }
            catch (TerminologyPollProcessingDependencyException terminologyPollProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollProcessingDependencyException);
            }
            catch (TerminologyPollProcessingServiceException terminologyPollProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(terminologyPollProcessingServiceException);
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
                var failedAddressCoordinationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration aggregate service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var FailedTerminologyMetadataOrchestrationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration service error occurred, " +
                            "please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(FailedTerminologyMetadataOrchestrationServiceException);
            }
        }

        private async ValueTask<TerminologyMetadataOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyMetadataOrchestrationValidationException);

            return terminologyMetadataOrchestrationValidationException;
        }

        private async ValueTask<TerminologyMetadataOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyMetadataOrchestrationDependencyValidationException =
                new TerminologyMetadataOrchestrationDependencyValidationException(
                    message:
                        "Terminology metadata orchestration dependency validation error occurred, " +
                            "fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyMetadataOrchestrationDependencyValidationException);

            return terminologyMetadataOrchestrationDependencyValidationException;
        }

        private async ValueTask<TerminologyMetadataOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var terminologyMetadataOrchestrationDependencyException =
                new TerminologyMetadataOrchestrationDependencyException(
                    message: "Terminology metadata orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(terminologyMetadataOrchestrationDependencyException);

            throw terminologyMetadataOrchestrationDependencyException;
        }

        private async ValueTask<TerminologyMetadataOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var terminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message: "Terminology metadata orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(terminologyMetadataOrchestrationServiceException);

            throw terminologyMetadataOrchestrationServiceException;
        }
    }
}