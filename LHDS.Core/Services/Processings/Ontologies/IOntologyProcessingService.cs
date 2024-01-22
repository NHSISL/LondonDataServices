// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Ontologies;

namespace LHDS.Core.Services.Processings.Ontologies
{
    internal interface IOntologyProcessingService
    {
        ValueTask<OntologyAssets> RetrieveAllCodingSystemsAsync(string relativeUrl);
        ValueTask<OntologyAssets> RetrieveAllValueSetsAsync(string relativeUrl);
        ValueTask<OntologyAssets> RetrieveAllConceptMapsAsync(string relativeUrl);
        ValueTask<string> RetrieveArtifactDetailsAsync(string relativeUrl);
    }
}
