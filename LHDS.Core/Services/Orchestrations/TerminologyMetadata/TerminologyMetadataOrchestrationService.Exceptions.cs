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
                throw CreateAndLogValidationException(invalidArgumentTerminologyMetaDataOrchestrationException);
            }
            catch (TerminologyPollProcessingValidationException terminologyPollProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyPollProcessingValidationException);
            }
            catch (TerminologyPollProcessingDependencyValidationException
                terminologyPollProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyPollProcessingDependencyValidationException);
            }
            catch (TerminologyArtifactProcessingValidationException terminologyArtifactProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(terminologyArtifactProcessingValidationException);
            }
            catch (TerminologyArtifactProcessingDependencyValidationException
                terminologyArtifactProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    terminologyArtifactProcessingDependencyValidationException);
            }
            catch (OntologyProcessingValidationException ontologyProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyProcessingValidationException);
            }
            catch (OntologyProcessingDependencyValidationException ontologyProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyProcessingDependencyValidationException);
            }
            catch (TerminologyPollProcessingDependencyException terminologyPollProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(terminologyPollProcessingDependencyException);
            }
            catch (TerminologyPollProcessingServiceException terminologyPollProcessingServiceException)
            {
                throw CreateAndLogDependencyException(terminologyPollProcessingServiceException);
            }
            catch (TerminologyArtifactProcessingDependencyException terminologyArtifactProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactProcessingDependencyException);
            }
            catch (TerminologyArtifactProcessingServiceException terminologyArtifactProcessingServiceException)
            {
                throw CreateAndLogDependencyException(terminologyArtifactProcessingServiceException);
            }
            catch (OntologyProcessingDependencyException ontologyProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(ontologyProcessingDependencyException);
            }
            catch (OntologyProcessingServiceException ontologyProcessingServiceException)
            {
                throw CreateAndLogDependencyException(ontologyProcessingServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration aggregate service error occurred, please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var FailedTerminologyMetadataOrchestrationServiceException =
                    new FailedTerminologyMetadataOrchestrationServiceException(
                        message: "Failed terminology metadata orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(FailedTerminologyMetadataOrchestrationServiceException);
            }
        }

        private TerminologyMetadataOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyMetadataOrchestrationValidationException);

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

            this.loggingBroker.LogError(terminologyMetadataOrchestrationDependencyValidationException);

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

            this.loggingBroker.LogError(terminologyMetadataOrchestrationDependencyException);

            throw terminologyMetadataOrchestrationDependencyException;
        }

        private TerminologyMetadataOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var terminologyMetadataOrchestrationServiceException =
                new TerminologyMetadataOrchestrationServiceException(
                    message: "Terminology metadata orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(terminologyMetadataOrchestrationServiceException);

            throw terminologyMetadataOrchestrationServiceException;
        }
    }
}