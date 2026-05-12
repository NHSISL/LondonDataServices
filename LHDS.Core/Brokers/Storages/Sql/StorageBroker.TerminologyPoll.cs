// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<TerminologyPoll> TerminologyPolls { get; set; }

        public async ValueTask<TerminologyPoll> InsertTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(terminologyPoll, cancellationToken);

        public async ValueTask<IQueryable<TerminologyPoll>> SelectAllTerminologyPollsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<TerminologyPoll>(cancellationToken);

        public async ValueTask<TerminologyPoll> SelectTerminologyPollByIdAsync(
            Guid terminologyPollId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<TerminologyPoll>(new object[] { terminologyPollId }, cancellationToken);

        public async ValueTask<TerminologyPoll> UpdateTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(terminologyPoll, cancellationToken);

        public async ValueTask<TerminologyPoll> DeleteTerminologyPollAsync(
            TerminologyPoll terminologyPoll,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(terminologyPoll, cancellationToken);
    }
}