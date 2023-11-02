// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OntologyValueSet> OntologyValueSets { get; set; }

        public async ValueTask<OntologyValueSet> InsertOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet) =>
                await InsertAsync(ontologyValueSet);

        public IQueryable<OntologyValueSet> SelectAllOntologyValueSets() => ReadAll<OntologyValueSet>();

        public async ValueTask<OntologyValueSet> SelectOntologyValueSetByIdAsync(
            Guid ontologyValueSetId) =>
                await ReadAsync<OntologyValueSet>(ontologyValueSetId);

        public async ValueTask<OntologyValueSet> UpdateOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet) =>
                await UpdateAsync(ontologyValueSet);

        public async ValueTask<OntologyValueSet> DeleteOntologyValueSetAsync(
            OntologyValueSet ontologyValueSet) =>
                await DeleteAsync(ontologyValueSet);
    }
}