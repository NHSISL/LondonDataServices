using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public interface IOntologyCodeSystemService
    {
        ValueTask<OntologyCodeSystem> AddOntologyCodeSystemAsync(OntologyCodeSystem ontologyCodeSystem);
        IQueryable<OntologyCodeSystem> RetrieveAllOntologyCodeSystems();
        ValueTask<OntologyCodeSystem> RetrieveOntologyCodeSystemByIdAsync(Guid ontologyCodeSystemId);
        ValueTask<OntologyCodeSystem> ModifyOntologyCodeSystemAsync(OntologyCodeSystem ontologyCodeSystem);
    }
}