// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OntologyConceptMap> InsertOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap);

        IQueryable<OntologyConceptMap> SelectAllOntologyConceptMaps();
        ValueTask<OntologyConceptMap> SelectOntologyConceptMapByIdAsync(Guid ontologyConceptMapId);

        ValueTask<OntologyConceptMap> UpdateOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap);

        ValueTask<OntologyConceptMap> DeleteOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap);
    }
}
