// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OntologyCodeSystem> OntologyCodeSystems { get; set; }

        public async ValueTask<OntologyCodeSystem> InsertOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem) =>
                await InsertAsync(ontologyCodeSystem);

        public IQueryable<OntologyCodeSystem> SelectAllOntologyCodeSystems() => ReadAll<OntologyCodeSystem>();

        public async ValueTask<OntologyCodeSystem> SelectOntologyCodeSystemByIdAsync(
            Guid ontologyCodeSystemId) =>
                await ReadAsync<OntologyCodeSystem>(ontologyCodeSystemId);

        public async ValueTask<OntologyCodeSystem> UpdateOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem) =>
                await UpdateAsync(ontologyCodeSystem);

        public async ValueTask<OntologyCodeSystem> DeleteOntologyCodeSystemAsync(
            OntologyCodeSystem ontologyCodeSystem) =>
                await DeleteAsync(ontologyCodeSystem);
    }
}