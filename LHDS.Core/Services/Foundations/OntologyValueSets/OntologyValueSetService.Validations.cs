using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetService
    {
        private void ValidateOntologyValueSetOnAdd(OntologyValueSet ontologyValueSet)
        {
            ValidateOntologyValueSetIsNotNull(ontologyValueSet);
        }

        private static void ValidateOntologyValueSetIsNotNull(OntologyValueSet ontologyValueSet)
        {
            if (ontologyValueSet is null)
            {
                throw new NullOntologyValueSetException(message: "OntologyValueSet is null.");
            }
        }
    }
}