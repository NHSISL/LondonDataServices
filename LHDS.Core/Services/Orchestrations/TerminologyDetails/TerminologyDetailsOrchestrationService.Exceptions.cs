// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                throw CreateAndLogDependencyValidationException(documentProcessingValidationException);
            }
            catch (DocumentProcessingDependencyValidationException documentProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentProcessingDependencyValidationException);
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
            catch (DocumentProcessingDependencyException documentProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(documentProcessingDependencyException);
            }
            catch (DocumentProcessingServiceException documentProcessingServiceException)
            {
                throw CreateAndLogDependencyException(documentProcessingServiceException);
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
            catch (Exception exception)
            {
                var FailedTerminologyDetailOrchestrationServiceException =
                    new FailedTerminologyDetailOrchestrationServiceException(
                        message: "Failed terminology detail orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(FailedTerminologyDetailOrchestrationServiceException);
            }
        }

        private TerminologyDetailOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var terminologyDetailOrchestrationDependencyValidationException =
                new TerminologyDetailOrchestrationDependencyValidationException(
                    message:
                        "Terminology detail orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyDetailOrchestrationDependencyValidationException);

            return terminologyDetailOrchestrationDependencyValidationException;
        }

        private TerminologyDetailOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var terminologyDetailOrchestrationDependencyException =
                new TerminologyDetailOrchestrationDependencyException(
                    message: "Terminology detail orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(terminologyDetailOrchestrationDependencyException);

            throw terminologyDetailOrchestrationDependencyException;
        }

        private TerminologyDetailOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var terminologyDetailOrchestrationServiceException =
                new TerminologyDetailOrchestrationServiceException(
                    message: "Terminology detail orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(terminologyDetailOrchestrationServiceException);

            throw terminologyDetailOrchestrationServiceException;
        }
    }
}