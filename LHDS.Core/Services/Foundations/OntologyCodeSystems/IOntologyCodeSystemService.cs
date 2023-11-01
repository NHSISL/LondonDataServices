using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public interface IOntologyCodeSystemService
    {
        ValueTask<OntologyCodeSystem> AddOntologyCodeSystemAsync(OntologyCodeSystem ontologyCodeSystem);
    }
}