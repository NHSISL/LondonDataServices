// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Ontologies
{
    public partial class OntologyProcessingService
    {
        private delegate ValueTask<OntologyAssets> ReturningOntologyAssetsFunction();
        private delegate ValueTask<String> ReturningStringFunction();

        private async ValueTask<OntologyAssets> TryCatch(ReturningOntologyAssetsFunction returningOntologyAssetsFunction)
        {
            try
            {
                return await returningOntologyAssetsFunction();
            }
            catch (InvalidArgumentOntologyProcessingException invalidArgumentOntologyProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentOntologyProcessingException);
            }
            catch (OntologyValidationException ontologyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyValidationException);
            }
            catch (OntologyDependencyValidationException ontologyDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyDependencyValidationException);
            }
            catch (OntologyDependencyException ontologyDependencyException)
            {
                throw CreateAndLogDependencyException(ontologyDependencyException);
            }
            catch (OntologyServiceException ontologyServiceException)
            {
                throw CreateAndLogDependencyException(ontologyServiceException);
            }
            catch (Exception exception)
            {
                var failedOntologyProcessingServiceException =
                    new FailedOntologyProcessingServiceException(
                        message: "Failed ontology processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyProcessingServiceException);
            }
        }

        private async ValueTask<String> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentOntologyProcessingException invalidArgumentOntologyProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentOntologyProcessingException);
            }
            catch (OntologyValidationException ontologyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyValidationException);
            }
            catch (OntologyDependencyValidationException ontologyDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ontologyDependencyValidationException);
            }
            catch (OntologyDependencyException ontologyDependencyException)
            {
                throw CreateAndLogDependencyException(ontologyDependencyException);
            }
            catch (OntologyServiceException ontologyServiceException)
            {
                throw CreateAndLogDependencyException(ontologyServiceException);
            }
            catch (Exception exception)
            {
                var failedOntologyProcessingServiceException =
                    new FailedOntologyProcessingServiceException(
                        message: "Failed ontology processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyProcessingServiceException);
            }
        }

        private OntologyProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyProcessingValidationException = new OntologyProcessingValidationException(
                message: "Ontology processing validation error occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(ontologyProcessingValidationException);

            return ontologyProcessingValidationException;
        }

        private OntologyProcessingDependencyValidationException CreateAndLogDependencyValidationException(
           Xeption exception)
        {
            var ontologyProcessingDependencyValidationException =
                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(ontologyProcessingDependencyValidationException);

            return ontologyProcessingDependencyValidationException;
        }

        private OntologyProcessingDependencyException CreateAndLogDependencyException(
           Xeption exception)
        {
            var ontologyProcessingDependencyException =
                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(ontologyProcessingDependencyException);

            return ontologyProcessingDependencyException;
        }

        private OntologyProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ontologyProcessingServiceException = new
                OntologyProcessingServiceException(
                    message: "Ontology processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyProcessingServiceException);

            return ontologyProcessingServiceException;
        }
    }
}
