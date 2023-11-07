// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Ontologies;
using LHDS.Core.Services.Foundations.Ontologies;

namespace LHDS.Core.Services.Processings.Ontologies
{
    internal partial class OntologyProcessingService : IOntologyProcessingService
    {
        private readonly IOntologyService ontologyService;
        private readonly ILoggingBroker loggingBroker;

        public OntologyProcessingService(
            IOntologyService ontologyService,
            ILoggingBroker loggingBroker)
        {
            this.ontologyService = ontologyService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<OntologyAssets> RetrieveAllCodingSystemsAsync(string relativeUrl) =>
             TryCatch( () =>
             {
                 ValidateArgs(relativeUrl);

                 return this.ontologyService.RetrieveAllCodingSystemsAsync(relativeUrl);
             });

        public ValueTask<OntologyAssets> RetrieveAllValueSetsAsync(string relativeUrl) =>
             TryCatch(() =>
             {
                 ValidateArgs(relativeUrl);
                 
                 return this.ontologyService.RetrieveAllValueSetsAsync(relativeUrl);
             });

        public ValueTask<OntologyAssets> RetrieveAllConceptMapsAsync(string relativeUrl) =>
            throw new System.NotImplementedException();
    }    

}
