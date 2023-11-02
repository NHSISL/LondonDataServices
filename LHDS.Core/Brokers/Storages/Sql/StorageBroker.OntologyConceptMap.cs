// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OntologyConceptMap> OntologyConceptMaps { get; set; }

        public async ValueTask<OntologyConceptMap> InsertOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap) =>
                await InsertAsync(ontologyConceptMap);

        public IQueryable<OntologyConceptMap> SelectAllOntologyConceptMaps() => ReadAll<OntologyConceptMap>();

        public async ValueTask<OntologyConceptMap> SelectOntologyConceptMapByIdAsync(
            Guid ontologyConceptMapId) =>
                await ReadAsync<OntologyConceptMap>(ontologyConceptMapId);

        public async ValueTask<OntologyConceptMap> UpdateOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap) =>
                await UpdateAsync(ontologyConceptMap);

        public async ValueTask<OntologyConceptMap> DeleteOntologyConceptMapAsync(
            OntologyConceptMap ontologyConceptMap) =>
                await DeleteAsync(ontologyConceptMap);
    }
}