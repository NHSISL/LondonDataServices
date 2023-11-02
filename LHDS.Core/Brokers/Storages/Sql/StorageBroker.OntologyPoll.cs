// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<OntologyPoll> OntologyPolls { get; set; }

        public async ValueTask<OntologyPoll> InsertOntologyPollAsync(
            OntologyPoll ontologyPoll) =>
                await InsertAsync(ontologyPoll);

        public IQueryable<OntologyPoll> SelectAllOntologyPolls() => ReadAll<OntologyPoll>();

        public async ValueTask<OntologyPoll> SelectOntologyPollByIdAsync(
            Guid ontologyPollId) =>
                await ReadAsync<OntologyPoll>(ontologyPollId);

        public async ValueTask<OntologyPoll> UpdateOntologyPollAsync(
            OntologyPoll ontologyPoll) =>
                await UpdateAsync(ontologyPoll);

        public async ValueTask<OntologyPoll> DeleteOntologyPollAsync(
            OntologyPoll ontologyPoll) =>
                await DeleteAsync(ontologyPoll);
    }
}