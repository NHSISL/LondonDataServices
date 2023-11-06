// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Models.Processings.Ontologies.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Ontologies
{
    internal partial class OntologyProcessingService
    {
        private delegate ValueTask<OntologyAssets> ReturningOntologyAssetsFunction();

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
        }

        private OntologyProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyProcessingValidationException = new OntologyProcessingValidationException(
                message: "Ontology processing validation error occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(ontologyProcessingValidationException);

            return ontologyProcessingValidationException;
        }
    }
}
