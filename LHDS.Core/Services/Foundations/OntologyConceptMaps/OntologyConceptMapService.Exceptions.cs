using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService
    {
        private delegate ValueTask<OntologyConceptMap> ReturningOntologyConceptMapFunction();

        private async ValueTask<OntologyConceptMap> TryCatch(ReturningOntologyConceptMapFunction returningOntologyConceptMapFunction)
        {
            try
            {
                return await returningOntologyConceptMapFunction();
            }
            catch (NullOntologyConceptMapException nullOntologyConceptMapException)
            {
                throw CreateAndLogValidationException(nullOntologyConceptMapException);
            }
        }

        private OntologyConceptMapValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapValidationException);

            return ontologyConceptMapValidationException;
        }
    }
}