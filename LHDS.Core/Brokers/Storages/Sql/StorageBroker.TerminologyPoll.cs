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
        public DbSet<TerminologyPoll> TerminologyPolls { get; set; }

        public async ValueTask<TerminologyPoll> InsertOntologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await InsertAsync(terminologyPoll);

        public IQueryable<TerminologyPoll> SelectAllOntologyPolls() => ReadAll<TerminologyPoll>();

        public async ValueTask<TerminologyPoll> SelectOntologyPollByIdAsync(
            Guid terminologyPollId) =>
                await ReadAsync<TerminologyPoll>(terminologyPollId);

        public async ValueTask<TerminologyPoll> UpdateOntologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await UpdateAsync(terminologyPoll);

        public async ValueTask<TerminologyPoll> DeleteOntologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await DeleteAsync(terminologyPoll);
    }
}