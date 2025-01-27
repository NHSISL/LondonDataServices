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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentOntologyProcessingException);
            }
            catch (OntologyValidationException ontologyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyValidationException);
            }
            catch (OntologyDependencyValidationException ontologyDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyDependencyValidationException);
            }
            catch (OntologyDependencyException ontologyDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyDependencyException);
            }
            catch (OntologyServiceException ontologyServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyServiceException);
            }
            catch (Exception exception)
            {
                var failedOntologyProcessingServiceException =
                    new FailedOntologyProcessingServiceException(
                        message: "Failed ontology processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOntologyProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentOntologyProcessingException);
            }
            catch (OntologyValidationException ontologyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyValidationException);
            }
            catch (OntologyDependencyValidationException ontologyDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ontologyDependencyValidationException);
            }
            catch (OntologyDependencyException ontologyDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyDependencyException);
            }
            catch (OntologyServiceException ontologyServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ontologyServiceException);
            }
            catch (Exception exception)
            {
                var failedOntologyProcessingServiceException =
                    new FailedOntologyProcessingServiceException(
                        message: "Failed ontology processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOntologyProcessingServiceException);
            }
        }

        private async ValueTask<OntologyProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var ontologyProcessingValidationException = new OntologyProcessingValidationException(
                message: "Ontology processing validation error occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(ontologyProcessingValidationException);

            return ontologyProcessingValidationException;
        }

        private async ValueTask<OntologyProcessingDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var ontologyProcessingDependencyValidationException =
                new OntologyProcessingDependencyValidationException(
                    message: "Ontology processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ontologyProcessingDependencyValidationException);

            return ontologyProcessingDependencyValidationException;
        }

        private async ValueTask<OntologyProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
           Xeption exception)
        {
            var ontologyProcessingDependencyException =
                new OntologyProcessingDependencyException(
                    message: "Ontology processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ontologyProcessingDependencyException);

            return ontologyProcessingDependencyException;
        }

        private async ValueTask<OntologyProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ontologyProcessingServiceException = new
                OntologyProcessingServiceException(
                    message: "Ontology processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ontologyProcessingServiceException);

            return ontologyProcessingServiceException;
        }
    }
}
