// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    public partial class OntologyService
    {
        private delegate ValueTask<OntologyAssets> ReturningOntologyAssetsFunction();
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<OntologyAssets> TryCatch(ReturningOntologyAssetsFunction returningOntologyAssetsFunction)
        {
            try
            {
                return await returningOntologyAssetsFunction();
            }
            catch (InvalidArgumentOntologyException invalidArgumentOntologyException)
            {
                throw CreateAndLogValidationException(invalidArgumentOntologyException);
            }
            catch (Exception exception)
            {
                var failedOntologyServiceException =
                    new FailedOntologyServiceException(
                        message: "Failed ontology service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentOntologyException invalidArgumentOntologyException)
            {
                throw CreateAndLogValidationException(invalidArgumentOntologyException);
            }
            catch (Exception exception)
            {
                var failedOntologyServiceException =
                    new FailedOntologyServiceException(
                        message: "Failed ontology service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyServiceException);
            }
        }

        private OntologyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyValidationException = new OntologyValidationException(
                message: "Ontology validation error occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(ontologyValidationException);

            return ontologyValidationException;
        }

        private OntologyServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ontologyServiceException = new OntologyServiceException(
                message: "Ontology service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(ontologyServiceException);

            return ontologyServiceException;
        }
    }
}
