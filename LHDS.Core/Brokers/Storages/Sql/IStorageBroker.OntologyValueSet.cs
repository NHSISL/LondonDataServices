// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyValueSets;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<OntologyValueSet> InsertOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet);

        IQueryable<OntologyValueSet> SelectAllOntologyValueSets();
        ValueTask<OntologyValueSet> SelectOntologyValueSetByIdAsync(Guid ontologyValueSetId);

        ValueTask<OntologyValueSet> UpdateOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet);

        ValueTask<OntologyValueSet> DeleteOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet);
    }
}
