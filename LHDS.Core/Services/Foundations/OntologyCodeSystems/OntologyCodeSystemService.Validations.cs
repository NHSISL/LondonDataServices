using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemService
    {
        private void ValidateOntologyCodeSystemOnAdd(OntologyCodeSystem ontologyCodeSystem)
        {
            ValidateOntologyCodeSystemIsNotNull(ontologyCodeSystem);
        }

        private static void ValidateOntologyCodeSystemIsNotNull(OntologyCodeSystem ontologyCodeSystem)
        {
            if (ontologyCodeSystem is null)
            {
                throw new NullOntologyCodeSystemException(message: "OntologyCodeSystem is null.");
            }
        }
    }
}