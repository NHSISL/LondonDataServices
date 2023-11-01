using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyValueSets;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public interface IOntologyValueSetService
    {
        ValueTask<OntologyValueSet> AddOntologyValueSetAsync(OntologyValueSet ontologyValueSet);
        IQueryable<OntologyValueSet> RetrieveAllOntologyValueSets();
        ValueTask<OntologyValueSet> RetrieveOntologyValueSetByIdAsync(Guid ontologyValueSetId);
        ValueTask<OntologyValueSet> ModifyOntologyValueSetAsync(OntologyValueSet ontologyValueSet);
    }
}