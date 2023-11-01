using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public interface IOntologyConceptMapService
    {
        ValueTask<OntologyConceptMap> AddOntologyConceptMapAsync(OntologyConceptMap ontologyConceptMap);
        IQueryable<OntologyConceptMap> RetrieveAllOntologyConceptMaps();
        ValueTask<OntologyConceptMap> RetrieveOntologyConceptMapByIdAsync(Guid ontologyConceptMapId);
        ValueTask<OntologyConceptMap> ModifyOntologyConceptMapAsync(OntologyConceptMap ontologyConceptMap);
        ValueTask<OntologyConceptMap> RemoveOntologyConceptMapByIdAsync(Guid ontologyConceptMapId);
    }
}