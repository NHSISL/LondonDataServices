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
                throw await CreateAndLogValidationException(invalidArgumentTerminologyMetaDataOrchestrationException);
            }
            catch (TerminologyPollProcessingValidationException terminologyPollProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationException(terminologyPollProcessingValidationException);
            }
            catch (TerminologyPollProcessingDependencyValidationException
                terminologyPollProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(terminologyPollProcessingDependencyValidationException);
            }
            catch (TerminologyArtifactProcessingValidationException terminologyArtifactProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationException(terminologyArtifactProcessingValidationException);
            }
            catch (TerminologyArtifactProcessingDependencyValidationException
                terminologyArtifactProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(
                    terminologyArtifactProcessingDependencyValidationException);
            }
            catch (OntologyProcessingValidationException ontologyProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationException(ontologyProcessingValidationException);
            }
            catch (OntologyProcessingDependencyValidationException ontologyProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(ontologyProcessingDependencyValidationException);
            }
            catch (TerminologyPollProcessingDependencyException terminologyPollProcessingDependencyException)
            {
                throw await CreateAndLogDependencyException(terminologyPollProcessingDependencyException);
            }
            catch (TerminologyPollProcessingServiceException terminologyPollProcessingServiceException)
            {
                throw await CreateAndLogDependencyException(terminologyPollProcessingServiceException);
            }
            catch (TerminologyArtifactProcessingDependencyException terminologyArtifactProcessingDependencyException)
            {
                throw await CreateAndLogDependencyException(terminologyArtifactProcessingDependencyException);
            }
            catch (TerminologyArtifactProcessingServiceException terminologyArtifactProcessingServiceException)
            {
                throw await CreateAndLogDependencyException(terminologyArtifactProcessingServiceException);
            }
            catch (OntologyProcessingDependencyException ontologyProcessingDependencyException)
            {
                throw await CreateAndLogDependencyException(ontologyProcessingDependencyException);
            }
            catch (OntologyProcessingServiceException ontologyProcessingServiceException)
            {
                throw await CreateAndLogDependencyException(ontologyProcessingServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration aggregate service error occurred, please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceException(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var FailedTerminologyMetadataOrchestrationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceException(FailedTerminologyMetadataOrchestrationServiceException);
            }
        }

        private TerminologyMetadataOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogError(terminologyMetadataOrchestrationValidationException);

            return terminologyMetadataOrchestrationValidationException;
        }

        private TerminologyMetadataOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var terminologyMetadataOrchestrationDependencyValidationException =
                new TerminologyMetadataOrchestrationDependencyValidationException(
                    message:
                        "Terminology metadata orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogError(terminologyMetadataOrchestrationDependencyValidationException);

            return terminologyMetadataOrchestrationDependencyValidationException;
        }

        private TerminologyMetadataOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var terminologyMetadataOrchestrationDependencyException =
                new TerminologyMetadataOrchestrationDependencyException(
                    message: "Terminology metadata orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogError(terminologyMetadataOrchestrationDependencyException);

            throw terminologyMetadataOrchestrationDependencyException;
        }

        private TerminologyMetadataOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var terminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message: "Terminology metadata orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogError(terminologyMetadataOrchestrationServiceException);

            throw terminologyMetadataOrchestrationServiceException;
        }
    }
}