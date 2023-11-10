// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<TerminologyPoll> TerminologyPolls { get; set; }

        public async ValueTask<TerminologyPoll> InsertTerminologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await InsertAsync(terminologyPoll);

        public IQueryable<TerminologyPoll> SelectAllTerminologyPolls() => ReadAll<TerminologyPoll>();

        public async ValueTask<TerminologyPoll> SelectTerminologyPollByIdAsync(
            Guid terminologyPollId) =>
                await ReadAsync<TerminologyPoll>(terminologyPollId);

        public async ValueTask<TerminologyPoll> UpdateTerminologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await UpdateAsync(terminologyPoll);

        public async ValueTask<TerminologyPoll> DeleteTerminologyPollAsync(
            TerminologyPoll terminologyPoll) =>
                await DeleteAsync(terminologyPoll);
    }
}