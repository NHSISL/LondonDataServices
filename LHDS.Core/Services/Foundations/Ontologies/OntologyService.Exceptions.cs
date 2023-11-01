// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Foundations.Ontologies.Exceptions;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Ontologies
{
    internal partial class OntologyService
    {
        private delegate ValueTask<OntologyAssets> ReturningOntologyAssetsFunction();

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
                        message: "Failed ontology service occurred, please contact support",
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
                message: "Ontology service error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(ontologyServiceException);

            return ontologyServiceException;
        }
    }
}
