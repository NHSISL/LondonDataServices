using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService
    {
        private void ValidateOntologyConceptMapOnAdd(OntologyConceptMap ontologyConceptMap)
        {
            ValidateOntologyConceptMapIsNotNull(ontologyConceptMap);
        }

        private static void ValidateOntologyConceptMapIsNotNull(OntologyConceptMap ontologyConceptMap)
        {
            if (ontologyConceptMap is null)
            {
                throw new NullOntologyConceptMapException(message: "OntologyConceptMap is null.");
            }
        }
    }
}